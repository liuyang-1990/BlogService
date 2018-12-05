using System;
using Blog.Model;
using Blog.Model.Settings;
using Blog.Repository.Dao;
using Microsoft.Extensions.Options;
using NLog;

namespace Blog.Repository.Implement
{
    public class BaseRepository<T> where T : BaseEntity, new()
    {
        protected readonly DbContext<T> Context;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        protected BaseRepository(IOptions<DbSetting> settings)
        {
            Context = new DbContext<T>(settings);
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual string GetPageList(int pageIndex, int pageSize)
        {
            return Context.Db.Queryable<T>().ToJsonPage(pageIndex, pageSize);
        }

        /// <summary>
        /// 获取详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual string GetDetail(int id)
        {
            var info = Context.CurrentDb.GetSingle(x => x.Id == id);
            return Context.Db.Utilities.SerializeObject(info);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Insert(T entity)
        {
            try
            {
                return Context.CurrentDb.Insert(entity);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            return Context.CurrentDb.Update(entity);
        }
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Delete(int id)
        {
            try
            {
                return Context.CurrentDb.DeleteById(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return false;
            }
        }
    }
}