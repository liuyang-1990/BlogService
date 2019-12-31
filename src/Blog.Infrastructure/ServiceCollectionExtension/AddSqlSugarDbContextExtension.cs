using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
                //过滤器
                sqlSugarClient.QueryFilter.Add(new SqlFilterItem()
                {
                    //单表全局过滤器
                    FilterValue = db => new SqlFilterResult() { Sql = " is_deleted=0" },
                    IsJoinQuery = false
                });
                //执行SQL 错误事件
                sqlSugarClient.Aop.OnError = exp =>
                {
                    var logger = o.GetService<ILogger<SqlSugarClient>>();
                    logger.LogError("[SQL]:" + exp.Sql + "[Parameters]:" + exp.Parametres);
                    logger.LogError(Environment.NewLine);
                    logger.LogError(exp.Message);
                };
                return sqlSugarClient;
            });
            return service;
        }
    }
}
