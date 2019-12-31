using Blog.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Blog.Api.Filters
{
    public class ServiceExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ServiceException exception)
            {
                var statusCode = exception.HttpStatusCode != default ? exception.HttpStatusCode : HttpStatusCode.InternalServerError;
                context.HttpContext.Response.StatusCode = (int)statusCode;
                context.Result = new JsonResult(new
                {
                    ResponseCode = exception.ResponseCode,
                    Msg = context.Exception.Message,
                    StatusCode = statusCode
                });
            }
            else
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Result = new JsonResult(new
                {
                    ResponseCode = HttpStatusCode.InternalServerError.ToString(),
                    Msg = context.Exception.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                });
            }
        }
    }
}
