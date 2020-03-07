using Blog.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.MiddleWares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class DataProtectMiddleware
    {
        private readonly RequestDelegate _next;
        public DataProtectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var keys = httpContext.Request.RouteValues.Keys.Where(x => x.Contains("id", StringComparison.OrdinalIgnoreCase));
            if (keys.Any())
            {
                foreach (var key in keys)
                {
                    var protectParam = httpContext.Request.RouteValues[key].ToString();
                    var unprotectParam = protectParam.ToDecrypted();
                    httpContext.Request.RouteValues[key] = unprotectParam;
                }
                await _next(httpContext);
                return;
            }
            if ((httpContext.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase)
                || httpContext.Request.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase))
                && httpContext.Request.ContentType.Contains("json"))
            {
                var originBodyStream = httpContext.Request.Body;
                var requestData = new StringBuilder();
                using (var ms = new MemoryStream())
                {
                    httpContext.Request.Body = ms;
                    using (var reader = new StreamReader(originBodyStream))
                    {
                        var bodyAsText = await reader.ReadToEndAsync(); //读取body
                        requestData.Append(bodyAsText);
                    }
                    var jToken = JToken.Parse(requestData.ToString());
                    UnprotectParams(jToken);
                    var buffer = Encoding.UTF8.GetBytes(jToken.ToString());
                    ms.Seek(0, SeekOrigin.Begin);
                    await ms.WriteAsync(buffer, 0, buffer.Length);
                    await ms.FlushAsync();
                    httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                    httpContext.Request.ContentLength = buffer.Length;
                    await _next(httpContext);
                    return;
                }
            }

            await _next(httpContext);
        }

        private void UnprotectParams(JToken token)
        {
            if (token is JArray array)
            {
                foreach (var j in array)
                {
                    if (j is JValue val)
                    {
                        if (array.Parent is JProperty property && property.Name.Contains("Id", StringComparison.OrdinalIgnoreCase))
                        {
                            var strJ = val.Value.ToString();
                            val.Value = strJ.ToDecrypted();
                        }
                    }
                    else
                    {
                        UnprotectParams(j);
                    }
                }
            }
            else if (token is JObject obj)
            {
                foreach (var property in obj.Children<JProperty>())
                {
                    if (property.Value is JArray)
                    {
                        UnprotectParams(property.Value);
                    }
                    else
                    {
                        if (property.Name.Contains("Id", StringComparison.OrdinalIgnoreCase))
                        {
                            var val = property.Value.ToString();
                            property.Value = val.ToDecrypted();
                        }
                        else
                        {
                            //if Value has child values
                            if (property.Value.HasValues)
                            {
                                UnprotectParams(property.Value);
                            }
                        }
                    }
                }
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class DataProtectMiddlewareExtensions
    {
        public static IApplicationBuilder UseDataProtectMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DataProtectMiddleware>();
        }
    }
}
