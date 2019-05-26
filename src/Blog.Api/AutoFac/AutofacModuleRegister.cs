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
            //注册Service中的对象,Service中的类要以Business结尾，否则注册失败
            builder.RegisterAssemblyTypes(GetAssemblyByName("Blog.Business")).Where(a => a.Name.EndsWith("Business"))
                .AsImplementedInterfaces().InstancePerLifetimeScope().EnableInterfaceInterceptors().InterceptedBy(typeof(BlogRedisCacheAOP));
            //注册Repository中的对象,Repository中的类要以Repository结尾，否则注册失败
            builder.RegisterAssemblyTypes(GetAssemblyByName("Blog.Repository")).Where(a => a.Name.EndsWith("Repository")).AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(GetAssemblyByName("Blog.Infrastructure")).Where(a => a.Name.EndsWith("Helper")).AsImplementedInterfaces();

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