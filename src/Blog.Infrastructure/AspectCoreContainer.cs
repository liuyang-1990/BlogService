using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Blog.Model;

namespace Blog.Infrastructure
{
    public class AspectCoreContainer
    {
        private static IServiceResolver resolver { get; set; }

        public static IServiceProvider BuildServiceProvider(IServiceCollection services, Action<IAspectConfiguration> configure = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.ConfigureDynamicProxy(configure);
            services.AddAspectCoreContainer();

            var container = services.ToServiceContainer();
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract &&
                   t.GetCustomAttributes<InjectorAttribute>() != null));

            var serviceGroup = types.SelectMany(service => service.GetCustomAttributes<InjectorAttribute>()
                .Select(x => new { Attr = x, Impl = service }));
            foreach (var service in serviceGroup)
            {
                container.AddType(service.Attr.ServiceType, service.Impl, service.Attr.LifeTime);
            }
            return resolver = container.Build();
        }

        public static T Resolve<T>()
        {
            return resolver.Resolve<T>();
        }
    }
}
