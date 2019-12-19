using AspectCore.DynamicProxy;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling;
using System;
using System.Threading.Tasks;

namespace Blog.Api.Interceptors
{
    public class MiniProfilerInterceptor : AbstractInterceptorAttribute
    {
        private readonly ILogger<MiniProfilerInterceptor> _logger;
        public MiniProfilerInterceptor(ILogger<MiniProfilerInterceptor> logger)
        {
            _logger = logger;
        }
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                MiniProfiler.Current.Step($"执行方法:{context.ProxyMethod.Name}()->");
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"执行方法:{context.ProxyMethod.Name}异常:" + ex.StackTrace);
                MiniProfiler.Current.CustomTiming("Errors：", ex.Message);
            }

        }
    }
}
