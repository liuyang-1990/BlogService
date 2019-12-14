using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blog.Infrastructure.DI
{
    public class CoreContainer
    {
        private static CoreContainer _currentInstance;
        public static CoreContainer Current => _currentInstance ?? new CoreContainer();

        private IServiceCollection _serviceCollection;

        private IServiceProvider _serviceProvider;
        internal CoreContainer()
        {
            _serviceCollection = Helper.BuildServiceDescriptionCollection();
            _currentInstance = this;
        }


        public void BuildServiceProvider(IServiceCollection serviceCollection)
        {
            //TODO
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