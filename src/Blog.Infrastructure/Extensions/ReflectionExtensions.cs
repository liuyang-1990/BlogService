using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Blog.Infrastructure.Extensions
{
    public static class ReflectionExtensions
    {
        public static readonly ConcurrentDictionary<Type, PropertyInfo[]> TypePropertyCache = new ConcurrentDictionary<Type, PropertyInfo[]>();
        internal static readonly ConcurrentDictionary<PropertyInfo, Func<object, object>> PropertyValueGetters = new ConcurrentDictionary<PropertyInfo, Func<object, object>>();
        internal static readonly ConcurrentDictionary<PropertyInfo, Action<object, object>> PropertyValueSetters = new ConcurrentDictionary<PropertyInfo, Action<object, object>>();

        public static Func<object, object> GetValueGetter(this PropertyInfo propertyInfo)
        {
            return PropertyValueGetters.GetOrAdd(propertyInfo, prop =>
            {
                var instance = Expression.Parameter(typeof(object), "obj");
                var getterCall = Expression.Call(propertyInfo.DeclaringType.IsValueType ?
                      Expression.Unbox(instance, propertyInfo.DeclaringType) :
                      Expression.Convert(instance, propertyInfo.DeclaringType), prop.GetGetMethod());
                var castToObject = Expression.Convert(getterCall, typeof(object));
                return Expression.Lambda(castToObject, instance).Compile() as Func<object, object>;
            });
        }

        public static Action<object, object> GetValueSetter(this PropertyInfo propertyInfo)
        {
            return PropertyValueSetters.GetOrAdd(propertyInfo, prop =>
            {
                var obj = Expression.Parameter(typeof(object), "o");
                var value = Expression.Parameter(typeof(object));
                // Note that we are using Expression.Unbox for value types and Expression.Convert for reference types
                var expr =
                    Expression.Lambda<Action<object, object>>(
                        Expression.Call(
                            propertyInfo.DeclaringType.IsValueType
                                ? Expression.Unbox(obj, propertyInfo.DeclaringType)
                                : Expression.Convert(obj, propertyInfo.DeclaringType),
                            propertyInfo.GetSetMethod(),
                            Expression.Convert(value, propertyInfo.PropertyType)),
                        obj, value);
                return expr.Compile();
            });
        }
    }
}