using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using Blog.Infrastructure.ServiceCollectionExtension.ParamProtection;

namespace Blog.Infrastructure.ServiceCollectionExtension
{
    public static class AddParamProtectionExtension
    {
        public static IDataProtectionBuilder AddParamProtection(this IDataProtectionBuilder builder, Action<ParamProtectionConfig> configAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (builder.Services == null)
            {
                throw new ArgumentNullException(nameof(builder.Services));
            }
            if (configAction == null)
            {
                throw new ArgumentNullException(nameof(configAction));
            }
            builder.Services.Configure(configAction);
            builder.Services.Configure<MvcOptions>(option =>
            {
                option.Filters.Add<ParamsProtectionActionFilter>();
              //  option.Filters.Add<ParamsProtectionResultFilter>();
            });
            return builder;
        }
    }
}