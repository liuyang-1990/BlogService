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
                var code = exception.HttpStatusCode != default(HttpStatusCode)
                     ? exception.HttpStatusCode
                     : HttpStatusCode.InternalServerError;
                context.Result = new JsonResult(new
                {
                    ResponseCode = code.ToString(),
                    Msg = context.Exception.Message,
                    StatusCode = exception.ResponseCode
                });
            }
            else
            {
                context.Result = new JsonResult(new
                {
                    ResponseCode = HttpStatusCode.InternalServerError.ToString(),
                    Msg = context.Exception.Message,
                    StatusCode = context.HttpContext.Response.StatusCode,
                });
            }
        }
    }
}
