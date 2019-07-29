using AspectCore.DynamicProxy;
using Blog.Infrastructure;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling;
using System;
using System.Threading.Tasks;

namespace Blog.Api.Interceptors
{
    public class MiniProfilerInterceptor : AbstractInterceptorAttribute
    {

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {

                MiniProfiler.Current.Step($"执行方法:{context.ProxyMethod.Name}()->");
                await next(context);
            }
            catch (Exception ex)
            {
                var logger = AspectCoreContainer.Resolve<ILogger<MiniProfilerInterceptor>>();
                logger.LogError(ex.StackTrace);
                MiniProfiler.Current.CustomTiming("Errors：", ex.Message);
            }

        }
    }
}
