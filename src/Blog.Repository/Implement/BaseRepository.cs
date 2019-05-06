using Blog.Model;
using Blog.Model.Request;
using Blog.Model.Settings;
using Blog.Model.ViewModel;
using Blog.Repository.Dao;
using Microsoft.Extensions.Options;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Repository.Implement
{
    public class BaseRepository<T> where T : BaseEntity, new()
    {
        protected readonly DbContext<T> Context;

        protected BaseRepository(IOptions<DbSetting> settings)
        {
            Context = new DbContext<T>(settings);
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="param"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual async Task<JsonResultModel<T>> GetPageList(GridParams param, Expression<Func<T, bool>> expression)
        {
            const int totalNumber = 0;
            var pageInfo = await Context.Db.Queryable<T>().WhereIF(expression != null, expression)
            .OrderByIF(!string.IsNullOrEmpty(param.SortField) && !string.IsNullOrEmpty(param.SortOrder), param.SortField + " " + param.SortOrder)
            .ToPageListAsync(param.PageNum, param.PageSize, totalNumber);
            return new JsonResultModel<T>()
            {
                Rows = pageInfo.Key,
                TotalRows = pageInfo.Value
            };
        }

        /// <summary>
        /// 获取详细信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<T> GetDetail(int id)
        {
            return await Context.Db.Queryable<T>().SingleAsync(x => x.Id == id);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<bool> Insert(T entity)
        {
            var changes = await Context.Db.Insertable(entity).ExecuteCommandAsync();
            return changes > 0;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<bool> Update(T entity)
        {
            entity.ModifyTime = DateTime.Now;
            return await Context.Db.Updateable(entity).IgnoreColumns(true).IgnoreColumns(it => it.CreateTime).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 根据主键删除(假删除)
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<bool> Delete(int id)
        {
            return await Context.Db.Updateable<T>().UpdateColumns(it => it.IsDeleted == 1).Where(it => it.Id == id).ExecuteCommandHasChangeAsync();
        }
    }
}