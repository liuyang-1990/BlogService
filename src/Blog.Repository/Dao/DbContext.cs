using System;
using System.Linq;
using Blog.Model.Settings;
using Microsoft.Extensions.Options;
using SqlSugar;

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

            //调式代码 用来打印SQL 
            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" + Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
        }
    }
}