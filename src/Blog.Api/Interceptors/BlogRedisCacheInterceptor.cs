using AspectCore.DynamicProxy;
using Blog.Infrastructure;
using Blog.Infrastructure.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Blog.Infrastructure.DI;

namespace Blog.Api.Interceptors
{
    public class BlogRedisCacheInterceptor : AbstractInterceptorAttribute
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var methodInfo = context.ImplementationMethod;
            //无返回值
            if (methodInfo.IsVoid() || methodInfo.IsTask())
            {
                await next(context);
                return;
            }
            //没有CachingAttribute
            if (!methodInfo.IsDefined(typeof(CachingAttribute), true))
            {
                await next(context);
                return;
            }
            var cachingAttribute = methodInfo.GetCustomAttribute<CachingAttribute>();
            //获取缓存key
            var key = string.IsNullOrEmpty(cachingAttribute.CacheKey) ? GetCustomKey(context) : cachingAttribute.CacheKey;

            var distributedCache = CoreContainer.Current.GetService<IDistributedCache>();

            //获取缓存值
            var value = await distributedCache.GetStringAsync(key);
            //没有值先执行方法,后存入缓存中
            if (string.IsNullOrEmpty(value))
            {
                await next(context);
                dynamic returnValue = context.ReturnValue;
                if (methodInfo.IsReturnTask()) returnValue = returnValue.Result;
                if (returnValue == null)
                {
                    return;
                }
                var val = JsonConvert.SerializeObject(returnValue);
                await distributedCache.SetAsync(key, val, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cachingAttribute.AbsoluteExpiration)
                });
                return;
            }
            //有值的话直接从缓存拿
            if (methodInfo.IsReturnTask())
            {
                dynamic result = JsonConvert.DeserializeObject(value, methodInfo.ReturnType.GenericTypeArguments[0]);
                context.ReturnValue = Task.FromResult(result);
            }
            else
            {
                context.ReturnValue = JsonConvert.DeserializeObject(value, methodInfo.ReturnType);
            }
        }

        private string GetCustomKey(AspectContext context)
        {
            var methodInfo = context.ImplementationMethod;
            var key = $"{methodInfo.DeclaringType.Namespace}:{methodInfo.DeclaringType.Name}:{methodInfo.Name}:" +
                      $"{GetKey(methodInfo, methodInfo.GetParameters(), context.Parameters)}";
            return key;
        }

        private string GetKey(MethodInfo methodInfo, ParameterInfo[] inputParameterInfos, object[] parameterValues)
        {
            var code = methodInfo.GetHashCode();
            code = inputParameterInfos.Aggregate(code, (current, argument) => current ^ argument.GetHashCode());
            code = parameterValues.Aggregate(code, (current, value) => current ^ value.GetHashCode());
            return code.ToString();
        }

    }
}
