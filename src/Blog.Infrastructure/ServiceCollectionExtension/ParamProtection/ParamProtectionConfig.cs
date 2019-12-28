﻿using Blog.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Blog.Infrastructure.ServiceCollectionExtension.ParamProtection
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