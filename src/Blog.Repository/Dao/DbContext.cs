using Blog.Model.Settings;
using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Linq;

namespace Blog.Repository.Dao
{
    public class DbContext<T> where T : class, new()
    {
        /// <summary>
        /// 用来处理事务多表查询和复杂的操作
        /// </summary>
        public SqlSugarClient Db;

        /// <summary>
        /// 用来处理T表的常用操作
        /// </summary>
        public SimpleClient<T> CurrentDb => new SimpleClient<T>(Db);

        public DbContext(IOptions<DbSetting> settings)
        {
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = settings.Value.ConnectionString,
                DbType = DbType.MySql,
                InitKeyType = InitKeyType.SystemTable,
                IsAutoCloseConnection = true
            });

            Db.QueryFilter.Add(new SqlFilterItem()
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
            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                var sqlP = sql + "\r\n" +
                           Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value));
                Console.WriteLine();
                Console.WriteLine(sqlP);
            };
        }
    }
}