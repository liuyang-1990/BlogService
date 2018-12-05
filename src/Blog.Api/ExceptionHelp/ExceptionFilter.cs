using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Blog.Api
{
    public class ExceptionFilter
    {
        private readonly RequestDelegate _next;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public ExceptionFilter(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception == null)
            {
                return;
            }
            await WriteExceptionAsync(context, exception).ConfigureAwait(false);
        }

        private async Task WriteExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.Error(exception.GetBaseException().ToString());
            var response = context.Response;
            //状态码
            if (exception is UnauthorizedAccessException)
            {
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            context.Response.ContentType = "application/json;charset=utf-8";
            var data = new { Code = response.StatusCode.ToString(), Success = false, Msg = exception.GetBaseException().ToString() };
            await response.WriteAsync(JsonConvert.SerializeObject(data)).ConfigureAwait(false);
        }
    }
}