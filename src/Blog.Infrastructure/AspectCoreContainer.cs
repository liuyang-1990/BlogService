using AspectCore.DependencyInjection;
using System;
using System.ComponentModel.Design;

namespace Blog.Infrastructure
{
    public class AspectCoreContainer
    {
        private static IServiceResolver _serviceResolver;
        public static void BuildServiceProvider(IServiceContainer containerBuilder)
        {
 // var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()
            //.Where(t => t.IsClass && !t.IsAbstract && t.IsDefined(typeof(InjectorAttribute), true)));

            // var serviceGroup = types.SelectMany(service => service.GetCustomAttributes<InjectorAttribute>()
            //     .Select(x => new { Attr = x, Impl = service }));
            // foreach (var service in serviceGroup)
            // {
            //     containerBuilder.AddType(service.Attr.ServiceType, service.Impl, service.Attr.LifeTime);
            // }
            // _serviceResolver = containerBuilder.Build();
        }

        public static T GetService<T>()
        {
            return _serviceResolver.Resolve<T>();
        }

        public static object GetService(Type type)
        {
            return _serviceResolver.Resolve(type);
        }
    }
}
