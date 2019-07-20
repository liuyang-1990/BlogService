using Blog.Infrastructure;
using SqlSugar;
using StackExchange.Profiling;
using System.Linq;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace Blog.Repository.Dao
{
    public class DbContext
    {
        /// <summary>
        /// 用来处理事务多表查询和复杂的操作
        /// </summary>
        private SqlSugarClient _db;


        private DbContext()
        {
            var configuration = CoreContainer.Instance.Resolve<IConfiguration>();
            _db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = configuration["ConnectionStrings:ConnectionString"],
                DbType = DbType.MySql,
                InitKeyType = InitKeyType.SystemTable,
                IsAutoCloseConnection = true
            });

            _db.QueryFilter.Add(new SqlFilterItem()
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
            _db.Aop.OnLogExecuting = (sql, pars) =>
            {
                var sqlP = sql + "\r\n" +
                           _db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value));
                MiniProfiler.Current.CustomTiming("[SQL]:", sqlP);
            };
        }

        public static SqlSugarClient GetDbContext()
        {

            var dbContext = new DbContext();
            return dbContext._db;

        }
    }
}