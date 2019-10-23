using Blog.Infrastructure;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SqlSugar;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Blog.Api.Filters
{
    public class DecryptReferenceFilter : IActionFilter
    {
        private readonly IDataProtector _dataProtector;
        private readonly IConfiguration _configuration;
        public DecryptReferenceFilter()
        {
            _dataProtector = AspectCoreContainer.Resolve<IDataProtectionProvider>().CreateProtector("protect_params");
            _configuration = AspectCoreContainer.Resolve<IConfiguration>();
        }



        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!_configuration["ParamsProtection:Enabled"].ObjToBool() || string.IsNullOrWhiteSpace(_configuration["ParamsProtection:Params"]))
            {
                return;
            }
            var request = context.HttpContext.Request;
            var protectionParams = _configuration["ParamsProtection:Params"].Split(",", StringSplitOptions.RemoveEmptyEntries);

            //QueryString
            if (request.Query != null && request.Query.Keys.Any())
            {
                foreach (var p in protectionParams)
                {
                    if (!request.Query.ContainsKey(p)) continue;
                    try
                    {
                        var protectParam = request.Query[p].ToString();
                        var unprotectParam = _dataProtector.Unprotect(protectParam);
                        context.ActionArguments[p] = unprotectParam;
                    }
                    catch (Exception)
                    {
                        context.Result = new BadRequestResult();
                    }
                }
            }
            //RouteData
            if (context.RouteData.Values.Keys.Any())
            {
                foreach (var p in protectionParams)
                {
                    if (!context.RouteData.Values.ContainsKey(p)) continue;
                    try
                    {
                        var protectParam = context.RouteData.Values[p].ToString();
                        var unprotectParam = _dataProtector.Unprotect(protectParam);
                        context.ActionArguments[p] = unprotectParam;
                    }
                    catch (Exception)
                    {
                        context.Result = new BadRequestResult();
                    }
                }
            }
            //RequestBody
            if (!request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase) &&
                !request.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase)) return;

            if (!request.ContentType.Contains("json"))
            {
                return;
            }

            request.EnableBuffering();
            var requestReader = new StreamReader(request.Body);
            request.Body.Position = 0;
            var requestContent = requestReader.ReadToEnd();
            var jToken = JToken.Parse(requestContent);
            try
            {
                UnprotectParams(jToken, _dataProtector, protectionParams);
                var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jToken));
                context.HttpContext.Request.Body = new MemoryStream(bytes);
            }
            catch (Exception)
            {
                context.Result = new BadRequestResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }


        private static void UnprotectParams(JToken token, IDataProtector protector, string[] protectionParams)
        {
            if (token is JArray array)
            {
                foreach (var j in array)
                {
                    if (j is JValue val)
                    {
                        var strJ = val.Value.ToString();
                        if (array.Parent is JProperty property && protectionParams.Any(x => x.Equals(property.Name, StringComparison.OrdinalIgnoreCase)))
                        {
                            val.Value = protector.Unprotect(strJ);
                        }
                    }
                    else
                    {
                        UnprotectParams(j, protector, protectionParams);
                    }
                }
            }
            else if (token is JObject obj)
            {
                foreach (var property in obj.Children<JProperty>())
                {
                    if (property.Value is JArray)
                    {
                        UnprotectParams(property.Value, protector, protectionParams);
                    }
                    else
                    {
                        if (protectionParams.Any(x => x.Equals(property.Name, StringComparison.OrdinalIgnoreCase)))
                        {
                            var val = property.Value.ToString();
                            property.Value = protector.Unprotect(val);
                        }
                    }
                }
            }

        }
    }
}