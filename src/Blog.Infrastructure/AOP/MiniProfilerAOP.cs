using Castle.DynamicProxy;
using StackExchange.Profiling;
using System;

namespace Blog.Infrastructure.AOP
{
    public class MiniProfilerAOP : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                MiniProfiler.Current.Step($"执行方法:{invocation.Method.Name}()->");
                invocation.Proceed();
            }
            catch (Exception e)
            {
                //执行的 service 中，收录异常
                MiniProfiler.Current.CustomTiming("Errors：", e.Message);
            }
        }
    }
}
