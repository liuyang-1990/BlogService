using System;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure.DI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class InjectorAttribute : Attribute
    {
        public InjectorAttribute(Type type)
        {
            ServiceType = type;
            ServiceLifetime = ServiceLifetime.Singleton;
        }
        public Type ServiceType { get; set; }

        public ServiceLifetime ServiceLifetime { get; set; }
    }
}
