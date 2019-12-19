using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Blog.Infrastructure.DI
{
    public class CoreContainer
    {
        private static CoreContainer _currentInstance;
        public static CoreContainer Current => _currentInstance ?? new CoreContainer();

        private readonly IServiceCollection _serviceCollection;

        private IServiceProvider _serviceProvider;
        internal CoreContainer()
        {
            _serviceCollection = Helper.BuildServiceDescriptionCollection();
            _currentInstance = this;
        }

        public void BuildServiceProvider(IServiceCollection serviceCollection)
        {
            foreach (var serviceDescriptor in _serviceCollection)
            {
                if (serviceCollection.FirstOrDefault(x => x.ServiceType == serviceDescriptor.ServiceType) == null)
                {
                    serviceCollection.Add(serviceDescriptor);
                }
            }
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        public object GetService(Type type)
        {
            return _serviceProvider.GetService(type);
        }

    }
}