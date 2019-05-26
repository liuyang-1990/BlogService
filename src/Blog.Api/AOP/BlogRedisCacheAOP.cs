using Blog.Infrastructure;
using Blog.Model.Attribute;
using Castle.DynamicProxy;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.AOP
{
    public class BlogRedisCacheAOP : IInterceptor
    {
        private readonly IRedisHelper _redisHelper;
        public BlogRedisCacheAOP(IRedisHelper redisHelper)
        {
            _redisHelper = redisHelper;
        }
        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;

            var attribute = method.GetCustomAttributes(typeof(CachingAttribute), true).FirstOrDefault() as CachingAttribute;
            if (attribute != null)
            {
                var key = GetCustomKey(invocation);
                var cacheValue = _redisHelper.Get(key);
                if (cacheValue != null)
                {
                    //将当前获取到的缓存值，赋值给当前执行方法
                    var type = invocation.Method.ReturnType;
                    var resultTypes = type.GenericTypeArguments;
                    if (type.FullName == "System.Void")
                    {
                        return;
                    }
                    object response;
                    if (typeof(Task).IsAssignableFrom(type))
                    {
                        //返回Task<T>
                        if (resultTypes.Any())
                        {
                            var resultType = resultTypes.FirstOrDefault();
                            // 核心1，直接获取 dynamic 类型
                            dynamic temp = JsonConvert.DeserializeObject(cacheValue, resultType);
                            response = Task.FromResult(temp);

                        }
                        else
                        {
                            //Task 无返回方法 指定时间内不允许重新运行
                            response = Task.Yield();
                        }
                    }
                    else
                    {
                        // 核心2，要进行 ChangeType
                        response = Convert.ChangeType(_redisHelper.Get<object>(key), type);
                    }
                    invocation.ReturnValue = response;
                }
                else
                {
                    //去执行当前的方法
                    invocation.Proceed();
                    object response;
                    //并且将当前结果存入缓存
                    var type = invocation.Method.ReturnType;
                    if (typeof(Task).IsAssignableFrom(type))
                    {
                        var result = type.GetProperty("Result");
                        response = result.GetValue(invocation.ReturnValue);
                    }
                    else
                    {
                        response = invocation.ReturnValue;
                    }
                    _redisHelper.Set(key, response, TimeSpan.FromMinutes(attribute.AbsoluteExpiration));
                }
            }
            else
            {
                invocation.Proceed();
            }
        }


        private static string GetCustomKey(IInvocation invocation)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            var arguments = invocation.Arguments.Take(3).Select(x => x.ObjToString()).ToList();
            return !arguments.Any() ? $"{typeName}_{methodName}" : $"{typeName}_{methodName}_{string.Join("_", arguments)}";
        }
    }
}