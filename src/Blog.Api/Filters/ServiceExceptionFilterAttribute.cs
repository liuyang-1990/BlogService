using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace Blog.Api.Filters
{
    public class ServiceExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception as UnauthorizedAccessException;
            var responseCode = HttpStatusCode.InternalServerError.ToString();
            if (exception != null)
            {
                responseCode = HttpStatusCode.Unauthorized.ToString();
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            var data = new
            {
                ResponseCode = responseCode,
                Msg = context.Exception.Message,
                StatusCode = context.HttpContext.Response.StatusCode,
            };
            context.Result = new JsonResult(data);

        }
    }
}
