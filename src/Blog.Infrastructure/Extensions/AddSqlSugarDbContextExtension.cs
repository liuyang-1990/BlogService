using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SqlSugar;

namespace Blog.Infrastructure.Extensions
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
            service.Add(ServiceDescriptor.Scoped<IDbContext, DbContext>());
            return service;
        }
    }

    public interface IDbContext
    {
        ISqlSugarClient GetDbContext();
    }

    public class DbContext : IDbContext
    {
        private readonly ConnectionConfig _connectionConfig;

        public DbContext(IOptions<ConnectionConfig> optionsAccessor)
        {
            if (optionsAccessor == null)
                throw new ArgumentNullException(nameof(optionsAccessor));
            _connectionConfig = optionsAccessor.Value;
        }
        public ISqlSugarClient GetDbContext()
        {
            var db = new SqlSugarClient(_connectionConfig);
            db.QueryFilter.Add(new SqlFilterItem()
            {
                //单表全局过滤器
                FilterValue = filterdb => new SqlFilterResult() {Sql = "   is_deleted=0"},
                IsJoinQuery = false
            });
            //.Add(new SqlFilterItem()
            //{
            //    //多表全局过滤器
            //    FilterValue = filterdb => new SqlFilterResult() { Sql = "   f.is_deleted=0" },
            //    IsJoinQuery = true
            //});
            return db;
        }
    }
}
