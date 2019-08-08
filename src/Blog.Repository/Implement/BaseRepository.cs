using Blog.Model;
using Blog.Model.Request;
using Blog.Model.ViewModel;
using Blog.Repository.Dao;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blog.Infrastructure;
using Blog.Infrastructure.Extensions;

namespace Blog.Repository.Implement
{
    public class BaseRepository<T> where T : BaseEntity, new()
    {
        protected ISqlSugarClient Db;

        protected BaseRepository()
        {
            Db = AspectCoreContainer.Resolve<IDbContext>().GetDbContext();
            //Db = DbContext.GetDbContext();
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="param"></param>
        /// <param name="expression"></param>
        /// <param name="ignoreColumns"></param>
        /// <returns></returns>
        public virtual async Task<JsonResultModel<T>> GetPageList(GridParams param, Expression<Func<T, bool>> expression, List<string> ignoreColumns = null)
        {
            var queryable = Db.Queryable<T>().WhereIF(expression != null, expression)
                .OrderByIF(!string.IsNullOrEmpty(param.SortField) && !string.IsNullOrEmpty(param.SortOrder),
                    param.SortField + " " + param.SortOrder);
            var ignore = GetIgnoreColumns(ignoreColumns);
            queryable.IgnoreColumns(ignore);
            return new JsonResultModel<T>()
            {
                Rows = await queryable.ToPageListAsync(param.PageNum, param.PageSize),
                TotalRows = await queryable.CountAsync()
            };
        }

        /// <summary>
        /// 获取详细信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="ignoreColumns"></param>
        /// <returns></returns>
        public virtual async Task<T> GetDetail(int id, List<string> ignoreColumns = null)
        {
            var ignore = GetIgnoreColumns(ignoreColumns);
            return await Db.Queryable<T>().IgnoreColumns(ignore).SingleAsync(x => x.Id == id);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<bool> Insert(T entity)
        {
            var changes = await Db.Insertable(entity).ExecuteCommandAsync();
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
            return await Db.Updateable(entity).IgnoreColumns(true).IgnoreColumns(it => it.CreateTime).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 根据主键删除(假删除)
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<bool> Delete(int id)
        {
            return await Db.Updateable<T>().SetColumns(it => it.IsDeleted == 1).Where(it => it.Id == id).ExecuteCommandHasChangeAsync();
        }

        private string[] GetIgnoreColumns(List<string> ignoreColumns)
        {
            var ignore = new List<string>() { "IsDeleted", "ModifyTime" };
            if (ignoreColumns != null && ignoreColumns.Any())
            {
                ignore.AddRange(ignoreColumns);
            }
            return ignore.ToArray();
        }
    }
}