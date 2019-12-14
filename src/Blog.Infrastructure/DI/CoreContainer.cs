using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Blog.Infrastructure.DI
{
    public class CoreContainer
    {
        public static void BuildServiceProvider(IServiceCollection serviceCollection)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsDefined(typeof(InjectorAttribute))));
            var serviceGroup = types.SelectMany(service => service.GetCustomAttributes<InjectorAttribute>()
                .Select(x => new { Attr = x, Impl = service }));
            foreach (var service in serviceGroup)
            {
                serviceCollection.Add(new ServiceDescriptor(service.Attr.ServiceType, service.Impl, service.Attr.Lifetime));
            }

        }

    }
}