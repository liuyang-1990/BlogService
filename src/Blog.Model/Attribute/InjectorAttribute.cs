using AspectCore.Injector;
using System;

namespace Blog.Model
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class InjectorAttribute : Attribute
    {
        public InjectorAttribute(Type type)
        {
            ServiceType = type;
            LifeTime = Lifetime.Singleton;
        }
        public Type ServiceType { get; set; }

        public Lifetime LifeTime { get; set; }
    }
}
