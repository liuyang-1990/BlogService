using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Blog.Infrastructure.Extensions.ParamProtection
{
    public class ParamsProtectionResultFilter : IResultFilter
    {
        private readonly IDataProtector _dataProtector;
        private readonly ParamProtectionConfig _protectionConfig;

        public ParamsProtectionResultFilter(IDataProtectionProvider provider, IOptions<ParamProtectionConfig> optionsAccessor)
        {
            _protectionConfig = optionsAccessor.Value;
            _dataProtector = provider.CreateProtector(_protectionConfig.Purpose);
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (!_protectionConfig.Enable || !_protectionConfig.Params.Any())
            {
                return;
            }

            foreach (var (key, value) in _protectionConfig.ProtectResponseValues)
            {
                if (key.IsInstanceOfType(context.Result))
                {
                    var prop = ReflectionExtensions.TypePropertyCache.GetOrAdd(key, t => t.GetProperties()).FirstOrDefault(p => p.Name == value);
                    var val = prop?.GetValueGetter()?.Invoke(context.Result);
                    if (val != null)
                    {
                        var obj = JToken.FromObject(val);
                        try
                        {
                            ProtectParams(obj);
                        }
                        catch (Exception)
                        {
                            context.Result = new BadRequestResult();
                        }
                        prop.GetValueSetter().Invoke(context.Result, obj);
                    }
                }
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {

        }


        private void ProtectParams(JToken token)
        {
            if (token is JArray array)
            {
                foreach (var j in array)
                {
                    if (array.Parent is JProperty property && j is JValue val)
                    {
                        var strJ = val.Value.ToString();
                        if (_protectionConfig.Params.Any(x => x.Equals(property.Name, StringComparison.OrdinalIgnoreCase)))
                        {
                            val.Value = _dataProtector.Protect(strJ);
                        }
                    }
                    else
                    {
                        ProtectParams(j);
                    }
                }
            }
            else if (token is JObject obj)
            {
                foreach (var property in obj.Children<JProperty>())
                {
                    var val = property.Value.ToString();
                    if (_protectionConfig.Params.Any(x => x.Equals(property.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        property.Value = _dataProtector.Protect(val);
                    }
                    else
                    {
                        if (property.Value.HasValues)
                        {
                            ProtectParams(property.Value);
                        }
                    }
                }
            }
        }
    }
}