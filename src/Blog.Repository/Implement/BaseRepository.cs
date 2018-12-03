using System;
using Blog.Infrastructure;
using Blog.Model;
using Blog.Repository.Dao;
using Microsoft.Extensions.Options;
using NLog;

namespace Blog.Repository.Implement
{
    public class BaseRepository<T> where T : BaseEntity, new()
    {
        protected readonly DbContext<T> Context;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        protected BaseRepository(IOptions<DBSetting> settings)
        {
            Context = new DbContext<T>(settings);
        }


        public virtual bool Insert(T entity)
        {
            try
            {
                return Context.Add(entity);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return false;
            }
        }


        public virtual bool Delete(int id)
        {
            try
            {
                return Context.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return false;
            }
        }
    }
}