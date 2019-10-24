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
                var parameterExpression = Expression.Parameter(typeof(object), "obj");
                return (Func<object, object>)Expression.Lambda(Expression.Convert(Expression.Call(propertyInfo.DeclaringType.IsValueType ? Expression.Unbox(parameterExpression, propertyInfo.DeclaringType) : Expression.Convert(parameterExpression, propertyInfo.DeclaringType), prop.GetGetMethod()), typeof(object)), parameterExpression).Compile();
            });
        }

        public static Action<object, object> GetValueSetter(this PropertyInfo propertyInfo)
        {
            var parameterExpression1 = Expression.Parameter(typeof(object), "obj");
            var parameterExpression2 = Expression.Parameter(typeof(object), "obj");
            return PropertyValueSetters.GetOrAdd(propertyInfo, prop => Expression.Lambda<Action<object, object>>(Expression.Call(propertyInfo.DeclaringType.IsValueType ? Expression.Unbox(parameterExpression1, propertyInfo.DeclaringType) : Expression.Convert(parameterExpression1, propertyInfo.DeclaringType), propertyInfo.GetSetMethod(), (Expression)Expression.Convert(parameterExpression2, propertyInfo.PropertyType)), parameterExpression1, parameterExpression2).Compile());
        }
    }
}