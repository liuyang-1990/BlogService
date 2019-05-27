using Autofac;
using Autofac.Extras.DynamicProxy;
using Blog.Api.AOP;
using System.Reflection;
using Module = Autofac.Module;

namespace Blog.Api.AutoFac
{
    public class AutofacModuleRegister : Module
    {
        //重写Autofac管道Load方法，在这里注册注入
        protected override void Load(ContainerBuilder builder)
        {
            //注册Service中的对象,
            builder.RegisterAssemblyTypes(GetAssemblyByName("Blog.Business"))
                .AsImplementedInterfaces().InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(BlogRedisCacheAOP), typeof(MiniProfilerAOP));

            //注册Repository中的对象
            builder.RegisterAssemblyTypes(GetAssemblyByName("Blog.Repository")).AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(GetAssemblyByName("Blog.Infrastructure")).AsImplementedInterfaces();

        }
        /// <summary>
        /// 根据程序集名称获取程序集
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns></returns>
        public static Assembly GetAssemblyByName(string assemblyName)
        {
            return Assembly.Load(assemblyName);
        }
    }
}