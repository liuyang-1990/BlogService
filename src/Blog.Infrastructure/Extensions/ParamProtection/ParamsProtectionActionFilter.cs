using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Infrastructure.Extensions.ParamProtection
{
    public class ParamsProtectionActionFilter : IActionFilter
    {
        private readonly IDataProtector _dataProtector;
        private readonly ParamProtectionConfig _protectionConfig;
        public ParamsProtectionActionFilter(IDataProtectionProvider provider, IOptions<ParamProtectionConfig> optionsAccessor)
        {
            _protectionConfig = optionsAccessor.Value;
            _dataProtector = provider.CreateProtector(_protectionConfig.Purpose);
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!_protectionConfig.Enable || !_protectionConfig.Params.Any())
            {
                return;
            }
            //no Arguments
            if (!context.ActionArguments.Keys.Any())
            {
                return;
            }
            var request = context.HttpContext.Request;
            //QueryString
            if (request.Query != null && request.Query.Keys.Any())
            {
                foreach (var p in _protectionConfig.Params)
                {
                    if (!request.Query.ContainsKey(p))
                    {
                        continue;
                    }
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
            if (context.RouteData.Values.Any())
            {
                foreach (var p in _protectionConfig.Params)
                {
                    if (!context.RouteData.Values.ContainsKey(p))
                    {
                        continue;
                    }
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
                !request.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            if (!request.ContentType.Contains("json"))
            {
                return;
            }
            var dic = new Dictionary<string, object>();
            foreach (var key in context.ActionArguments.Keys)
            {
                var json = JsonConvert.SerializeObject(context.ActionArguments[key]);
                var jToken = JToken.Parse(json);
                try
                {
                    UnprotectParams(jToken);
                    var val = JsonConvert.DeserializeObject(jToken.ToString(), context.ActionArguments[key].GetType());
                    dic.Add(key, val);
                }
                catch (Exception)
                {
                    context.Result = new BadRequestResult();
                }
            }
            foreach (var key in dic.Keys)
            {
                context.ActionArguments[key] = dic[key];
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }


        private void UnprotectParams(JToken token)
        {
            if (token is JArray array)
            {
                foreach (var j in array)
                {
                    if (j is JValue val)
                    {
                        if (array.Parent is JProperty property && _protectionConfig.Params.Any(x => x.Equals(property.Name, StringComparison.OrdinalIgnoreCase)))
                        {
                            var strJ = val.Value.ToString();
                            val.Value = _dataProtector.Unprotect(strJ);
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
                        if (_protectionConfig.Params.Any(x => x.Equals(property.Name, StringComparison.OrdinalIgnoreCase)))
                        {
                            var val = property.Value.ToString();
                            property.Value = _dataProtector.Unprotect(val);
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
}