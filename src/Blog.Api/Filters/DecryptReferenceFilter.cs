using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Api.Filters
{
    public class DecryptReferenceFilter : IActionFilter
    {
        private readonly IDataProtector _dataProtector;
        private readonly IConfiguration _configuration;
        public DecryptReferenceFilter(IDataProtectionProvider provider, IConfiguration configuration)
        {
            _dataProtector = provider.CreateProtector("protect_params");
            _configuration = configuration;
        }



        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!_configuration["ParamsProtection:Enabled"].ObjToBool() || string.IsNullOrWhiteSpace(_configuration["ParamsProtection:Params"]))
            {
                return;
            }
            //no Arguments
            if (!context.ActionArguments.Keys.Any())
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
            if (context.RouteData.Values.Keys.Any())
            {
                foreach (var p in protectionParams)
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
                    UnprotectParams(jToken, _dataProtector, protectionParams);
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


        private static void UnprotectParams(JToken token, IDataProtector protector, string[] protectionParams)
        {
            if (token is JArray array)
            {
                foreach (var j in array)
                {
                    if (j is JValue val)
                    {
                        if (array.Parent is JProperty property && protectionParams.Any(x => x.Equals(property.Name, StringComparison.OrdinalIgnoreCase)))
                        {
                            var strJ = val.Value.ToString();
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
                        else
                        {
                            //if Value has child values
                            if (property.Value.HasValues)
                            {
                                UnprotectParams(property.Value, protector, protectionParams);
                            }
                        }
                    }
                }
            }

        }
    }
}