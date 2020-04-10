using Blog.Infrastructure.DI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blog.Infrastructure
{
    public static class Helper
    {
        public static IServiceCollection BuildServiceDescriptionCollection()
        {
            var serviceCollection = new ServiceCollection();
            var serviceDescriptions = ScanServiceDescriptionMetas();
            foreach (var serviceDescription in serviceDescriptions)
            {
                serviceCollection.Add(serviceDescription);
            }
            return serviceCollection;
        }

        /// <summary>
        /// Scans the service description metas.
        /// </summary>
        /// <returns>The service description metas.</returns>
        private static IEnumerable<ServiceDescriptor> ScanServiceDescriptionMetas()
        {
            var serviceDescriptors = new List<ServiceDescriptor>();
            var assemblies = DependencyContext.Default.RuntimeLibraries.Where(o => o.Name.StartsWith("Blog.")).Select(o => Assembly.Load(new AssemblyName(o.Name))).ToList();
            var current = AppDomain.CurrentDomain.GetAssemblies();
            assemblies.AddRange(current);
            var types = assemblies.SelectMany(x => x.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsDefined(typeof(InjectorAttribute))));
            var serviceGroup = types.SelectMany(service => service.GetCustomAttributes<InjectorAttribute>()
                .Select(x => new { Attr = x, Impl = service }));
            foreach (var service in serviceGroup)
            {
                serviceDescriptors.Add(new ServiceDescriptor(service.Attr.ServiceType, service.Impl, service.Attr.Lifetime));
            }
            return serviceDescriptors;
        }

    }
}