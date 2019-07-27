using AspectCore.Injector;
using Autofac;
using Blog.Infrastructure;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using StackExchange.Profiling;
using System.Linq;

namespace Blog.Repository.Dao
{
    public class DbContext
    {

        public static SqlSugarClient GetDbContext()
        {
            var configuration = AspectCoreContainer.Resolve<IConfiguration>();
            //用来处理事务多表查询和复杂的操作
            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = configuration["ConnectionStrings:ConnectionString"],
                DbType = DbType.MySql,
                InitKeyType = InitKeyType.SystemTable,
                IsAutoCloseConnection = true
            });

            db.QueryFilter.Add(new SqlFilterItem()
            {
                //单表全局过滤器
                FilterValue = filterdb => new SqlFilterResult() { Sql = "   is_deleted=0" },
                IsJoinQuery = false
            }).Add(new SqlFilterItem()
            {
                //多表全局过滤器
                FilterValue = filterdb => new SqlFilterResult() { Sql = "   f.is_deleted=0" },
                IsJoinQuery = true
            });
            //调式代码 用来打印SQL 
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                var sqlP = sql + "\r\n" +
                           db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value));
                MiniProfiler.Current.CustomTiming("[SQL]:", sqlP);
            };
            return db;

        }
    }
}