using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Infrastructure;
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

        public DbContext(IOptions<DBSetting> settings)
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


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Add(T obj)
        {
            return CurrentDb.Insert(obj);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Delete(dynamic id)
        {
            return CurrentDb.DeleteById(id);
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Update(T obj)
        {
            return CurrentDb.Update(obj);
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetList()
        {
            return CurrentDb.GetList();
        }

    }
}