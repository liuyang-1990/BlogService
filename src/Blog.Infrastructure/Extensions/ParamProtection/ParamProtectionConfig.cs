using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Infrastructure.Extensions.ParamProtection
{
    public class ParamProtectionConfig
    {
        public bool Enable { get; set; }

        public string Purpose { get; set; }

        public string[] Params { get; set; }

        internal IDictionary<Type, string> ProtectResponseValues { get; } = new Dictionary<Type, string>()
        {
            {typeof(ObjectResult), "Value"}
        };

        public void AddProtectValue<TResult>(Expression<Func<TResult, object>> valueExpression) where TResult : class, IActionResult
        {
            ProtectResponseValues[typeof(TResult)] = valueExpression.GetMemberInfo().Name;
        }
    }
}
