using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blog.Infrastructure
{
    public static class CoreContainer
    {
        /// <summary>
        /// 容器实例
        /// </summary>
        public static IContainer Instance;

        public static IServiceProvider Init(IServiceCollection services, Func<ContainerBuilder, ContainerBuilder> func = null)
        {
            //新建容器构建器，用于注册组件和服务
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.AutofacBuild();
            func?.Invoke(builder);
            //利用构建器创建容器
            Instance = builder.Build();
            //第三方IOC接管core内置DI容器 
            return new AutofacServiceProvider(Instance);
        }

        public static void AutofacBuild(this ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(GetAssemblies())
                .Where(x => x.Name.EndsWith("Business") || x.Name.EndsWith("Repository") || x.Name.EndsWith("Helper"))
                .PublicOnly()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            //  .EnableInterfaceInterceptors()
            // .InterceptedBy(typeof(BlogRedisCacheAOP), typeof(MiniProfilerAOP));

        }


        public static Assembly[] GetAssemblies()
        {
            var assemblies = new List<Assembly>();
            assemblies.Add(Assembly.GetExecutingAssembly());
            assemblies.Add(Assembly.Load("Blog.Business"));
            assemblies.Add(Assembly.Load("Blog.Repository"));
            return assemblies.ToArray();
        }

    }
}
