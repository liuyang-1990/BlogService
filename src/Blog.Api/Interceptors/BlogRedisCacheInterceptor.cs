using AspectCore.DynamicProxy;
using Blog.Model;
using System;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
using Blog.Infrastructure;
using Newtonsoft.Json;

namespace Blog.Api.Interceptors
{
    public class BlogRedisCacheInterceptor : AbstractInterceptorAttribute
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var method = context.ImplementationMethod;
            if (method.GetCustomAttributes<CachingAttribute>().FirstOrDefault() is CachingAttribute attribute)
            {
                var redisHelper = AspectCoreContainer.Resolve<IRedisHelper>();
                var key = string.IsNullOrEmpty(attribute.CacheKey) ?
                    GetCustomKey(context) : attribute.CacheKey;
                var value = redisHelper.Get(key);
                if (value != null)
                {
                    if (method.IsReturnTask())
                    {
                        dynamic result = JsonConvert.DeserializeObject(value, method.ReturnType.GenericTypeArguments[0]);
                        context.ReturnValue = Task.FromResult(result);
                    }
                    else
                    {
                        context.ReturnValue = JsonConvert.DeserializeObject(value, method.ReturnType);
                    }
                }
                else
                {
                    await next(context);
                    dynamic returnValue = context.ReturnValue;
                    if (method.IsReturnTask())
                        returnValue = returnValue.Result;
                    redisHelper.Set(key, returnValue, TimeSpan.FromMinutes(attribute.AbsoluteExpiration));
                }
            }
            else
            {
                await next(context);
            }
        }

        private static string GetCustomKey(AspectContext context)
        {
            var method = context.ServiceMethod;

            var key = $"{method.DeclaringType.Namespace}:{method.DeclaringType.Name}:{method.Name}";
            if (context.Parameters != null && context.Parameters.Any())
            {
                var param = string.Join(":", context.Parameters);
                key += param;
            }
            return key;
        }
    }
}
