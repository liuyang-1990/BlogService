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

namespace Blog.Api.Filters
{
    public class DecryptReferenceFilter : IActionFilter
    {
        private readonly IDataProtector _dataProtector;
        protected readonly IConfiguration _configuration;
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
            if (context.RouteData.Values.ContainsKey("id"))
            {
                var param = context.RouteData.Values["id"].ToString();
                try
                {
                    var id = _dataProtector.Unprotect(param);
                    context.ActionArguments["id"] = id;
                }
                catch (Exception)
                {
                    context.Result = new BadRequestResult();
                }
            }
            var request = context.HttpContext.Request;
            if (request.Query != null && request.Query.ContainsKey("id"))
            {
                var param = request.Query["id"].ToString();
                try
                {
                    var id = _dataProtector.Unprotect(param);
                    context.ActionArguments["id"] = id;
                }
                catch (Exception)
                {
                    context.Result = new BadRequestResult();
                }
            }

            if (request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase)
              || request.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase))
            {
                if (!request.ContentType.Contains("json"))
                {
                    return;
                }
                request.EnableBuffering();
                var requestReader = new StreamReader(request.Body);
                request.Body.Position = 0;
                var requestContent = requestReader.ReadToEnd();
                var json = JToken.Parse(requestContent);


                var param = json.Value<string>("Id");
                if (string.IsNullOrEmpty(param))
                {
                    return;
                }
                try
                {
                    var id = _dataProtector.Unprotect(param);
                    context.ActionArguments["Id"] = id;
                }
                catch (Exception)
                {
                    context.Result = new BadRequestResult();
                }

            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }


        private static void UnprotectParams(JToken token, IDataProtector protector)
        {
            if (token is JArray array)
            {
                foreach (var j in array)
                {
                    if (j is JValue val)
                    {
                        var strJ = val.Value.ToString();
                        if (array.Parent is JProperty property)
                        {
                            val.Value = protector.Unprotect(strJ);
                        }
                    }
                    else
                    {
                        UnprotectParams(j, protector);
                    }
                }
            }
            else if (token is JObject obj)
            {
                foreach (var property in obj.Children<JProperty>())
                {
                    if (property.Value is JArray)
                    {
                        UnprotectParams(property.Value, protector);
                    }
                    else
                    {
                        var val = property.Value.ToString();
                        property.Value = protector.Unprotect(val);
                    }
                }
            }

        }
    }
}