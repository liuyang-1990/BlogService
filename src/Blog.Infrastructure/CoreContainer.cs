﻿using Autofac;
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
            //注册拦截器
            //builder.RegisterType<BlogRedisCacheAOP>();
            //builder.RegisterType<MiniProfilerAOP>();
            builder.AutofacBuild();
            func?.Invoke(builder);
            //利用构建器创建容器
            Instance = builder.Build();
            //第三方IOC接管core内置DI容器 
            return new AutofacServiceProvider(Instance);
        }

        private static void AutofacBuild(this ContainerBuilder builder)
        {

            builder.RegisterAssemblyTypes(GetAssemblies())
                   .PublicOnly()
                   .Where(x => x.IsClass)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
        }


        private static Assembly[] GetAssemblies()
        {
            var assemblies = new List<Assembly>();
            assemblies.Add(Assembly.GetExecutingAssembly());
            assemblies.Add(Assembly.Load("Blog.Business"));
            assemblies.Add(Assembly.Load("Blog.Repository"));
            return assemblies.ToArray();
        }

    }
}
