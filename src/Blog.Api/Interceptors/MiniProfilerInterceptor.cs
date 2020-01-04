using AspectCore.DynamicProxy;
using StackExchange.Profiling;
using System.Threading.Tasks;

namespace Blog.Api.Interceptors
{
    public class MiniProfilerInterceptor : AbstractInterceptorAttribute
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            MiniProfiler.Current.Step($"执行方法:{context.ProxyMethod.Name}()->");
            await next(context);
        }
    }
}
