using Blog.Infrastructure.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Blog.Api.Filters
{
    public class ParamsProtectionResultFilter : IAsyncResultFilter
    {
        private readonly IDataProtector _dataProtector;
        private readonly IConfiguration _configuration;

        public ParamsProtectionResultFilter(IDataProtectionProvider provider, IConfiguration configuration)
        {
            _dataProtector = provider.CreateProtector("protect_params");
            _configuration = configuration;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (_configuration["ParamsProtection:Enabled"] == "false" || string.IsNullOrWhiteSpace(_configuration["ParamsProtection:Params"]))
            {
                await next();
                return;
            }
            var protectionParams = _configuration["ParamsProtection:Params"].Split(",", StringSplitOptions.RemoveEmptyEntries);
            var prop = ReflectionExtensions.TypePropertyCache.GetOrAdd(typeof(ObjectResult), t => t.GetProperties()).FirstOrDefault(p => p.Name == "Value");
            var val = prop?.GetValueGetter()?.Invoke(context.Result);
            if (val != null)
            {
                var obj = JToken.FromObject(val);
                ProtectParams(obj, protectionParams);
                prop.GetValueSetter().Invoke(context.Result, obj);
            }
            await next();
            #region 另一种方案，暂未实现
            //context.HttpContext.Response.Body = new MemoryWrappedHttpResponseStream(context.HttpContext.Response.Body);
            //await next();
            //context.HttpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            //var str = await ReadStreamAsync(context.HttpContext.Response.Body).ConfigureAwait(false);
            //if (string.IsNullOrEmpty(str))
            //{
            //    return;
            //}
            //var jToken = JToken.Parse(str);
            //ProtectParams(jToken, protectionParams);
            //var buffer = Encoding.UTF8.GetBytes(jToken.ToString());
            //await context.HttpContext.Response.Body.WriteAsync(buffer, 0, buffer.Length); 
            #endregion
        }

        private void ProtectParams(JToken token, string[] protectionParams)
        {
            if (token is JArray array)
            {
                foreach (var j in array)
                {
                    if (array.Parent is JProperty property && j is JValue val)
                    {
                        var strJ = val.Value.ToString();
                        if (protectionParams.Any(x => x.Equals(property.Name, StringComparison.OrdinalIgnoreCase)))
                        {
                            val.Value = _dataProtector.Protect(strJ);
                        }
                    }
                    else
                    {
                        ProtectParams(j, protectionParams);
                    }
                }
            }
            else if (token is JObject obj)
            {
                foreach (var property in obj.Children<JProperty>())
                {
                    var val = property.Value.ToString();
                    if (protectionParams.Any(x => x.Equals(property.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        property.Value = _dataProtector.Protect(val);
                    }
                    else
                    {
                        if (property.Value.HasValues)
                        {
                            ProtectParams(property.Value, protectionParams);
                        }
                    }
                }
            }
        }


        private async Task<string> ReadStreamAsync(Stream stream)
        {
            using (var sr = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
            {
                var str = await sr.ReadToEndAsync();
                stream.Seek(0, SeekOrigin.Begin);
                return str;
            }
        }
    }


    public class MemoryWrappedHttpResponseStream : MemoryStream
    {
        private readonly Stream _innerStream;
        public MemoryWrappedHttpResponseStream(Stream innerStream)
        {
            _innerStream = innerStream ?? throw new ArgumentNullException(nameof(innerStream));
        }
        public override void Flush()
        {
            _innerStream.Flush();
            base.Flush();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            base.Write(buffer, offset, count);
            _innerStream.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _innerStream.Dispose();
            }
        }

        public override void Close()
        {
            base.Close();
            _innerStream.Close();
        }
    }
}