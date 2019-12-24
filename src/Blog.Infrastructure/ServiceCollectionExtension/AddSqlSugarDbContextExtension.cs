using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SqlSugar;
using System;

namespace Blog.Infrastructure.ServiceCollectionExtension
{
    public static class AddSqlSugarDbContextExtension
    {
        public static IServiceCollection AddSqlSugarDbContext(this IServiceCollection service, Action<ConnectionConfig> configAction)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }
            if (configAction == null)
            {
                throw new ArgumentNullException(nameof(configAction));
            }
            service.Configure(configAction);
            service.AddScoped(o =>
            {
                var optionsAccessor = o.GetService<IOptions<ConnectionConfig>>();
                if (optionsAccessor == null)
                {
                    throw new ArgumentNullException(nameof(optionsAccessor));
                }
                ISqlSugarClient sqlSugarClient = new SqlSugarClient(optionsAccessor.Value);
                sqlSugarClient.QueryFilter.Add(new SqlFilterItem()
                {
                    //单表全局过滤器
                    FilterValue = db => new SqlFilterResult() { Sql = " is_deleted=0" },
                    IsJoinQuery = false
                });
                return sqlSugarClient;
            });
            return service;
        }
    }
}
