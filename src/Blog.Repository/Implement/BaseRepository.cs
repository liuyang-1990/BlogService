using Blog.Infrastructure;
using Blog.Infrastructure.Extensions;
using Blog.Model;
using Blog.Model.Request;
using Blog.Model.ViewModel;
using Microsoft.AspNetCore.DataProtection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Repository.Implement
{
    public class BaseRepository<T> where T : BaseEntity, new()
    {
        protected ISqlSugarClient Db;

        protected IDataProtector DataProtector;

        protected BaseRepository()
        {
            Db = AspectCoreContainer.Resolve<IDbContext>().GetDbContext();
            DataProtector = AspectCoreContainer.Resolve<IDataProtectionProvider>().CreateProtector("protect id");
        }

        #region Query

        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <returns></returns>
        public virtual async Task<bool> QueryIsExist(Expression<Func<T, bool>> whereExpression)
        {
            return await Db.Queryable<T>().AnyAsync(whereExpression);
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> QueryAll()
        {
            return await Db.Queryable<T>().ToListAsync();
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public virtual async Task<List<T>> QueryAll(string strWhere)
        {
            return await Db.Queryable<T>().WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToListAsync();
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <returns></returns>
        public virtual async Task<List<T>> QueryAll(Expression<Func<T, bool>> whereExpression)
        {
            return await Db.Queryable<T>().WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param">分页以及排序参数</param>
        /// <param name="whereExpression">条件</param>
        /// <returns></returns>
        public virtual async Task<JsonResultModel<T>> QueryByPage(GridParams param, Expression<Func<T, bool>> whereExpression)
        {
            RefAsync<int> totalCount = 0;
            var queryable = Db.Queryable<T>().WhereIF(whereExpression != null, whereExpression)
                .OrderByIF(!string.IsNullOrEmpty(param.SortField) && !string.IsNullOrEmpty(param.SortOrder),
                    param.SortField + " " + param.SortOrder)
            .Mapper(it => { it.Id = DataProtector.Protect(it.Id.ToString()); });
            return new JsonResultModel<T>()
            {
                Rows = await queryable.ToPageListAsync(param.PageNum, param.PageSize, totalCount),
                TotalRows = totalCount
            };
        }

        /// <summary>
        ///  分页查询
        /// </summary>
        /// <param name="param">分页以及排序参数</param>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public virtual async Task<JsonResultModel<T>> QueryByPage(GridParams param, string strWhere)
        {
            var queryable = Db.Queryable<T>().WhereIF(string.IsNullOrEmpty(strWhere), strWhere)
                .OrderByIF(!string.IsNullOrEmpty(param.SortField) && !string.IsNullOrEmpty(param.SortOrder),
                    param.SortField + " " + param.SortOrder)
                .Mapper(it => { it.Id = DataProtector.Protect(it.Id.ToString()); });
            return new JsonResultModel<T>()
            {
                Rows = await queryable.ToPageListAsync(param.PageNum, param.PageSize),
                TotalRows = await queryable.CountAsync()
            };
        }


        /// <summary>
        /// 根据where条件查询一条数据
        /// </summary>
        /// <param name="whereExpression">where条件</param>
        /// <returns></returns>
        public virtual async Task<T> QueryByWhere(Expression<Func<T, bool>> whereExpression)
        {
            return await Db.Queryable<T>().FirstAsync(whereExpression);
        }

        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<T> QueryById(string id)
        {
            var unProtectId = DataProtector.Unprotect(id);
            return await Db.Queryable<T>().Mapper(x => x.Id = DataProtector.Protect(x.Id)).FirstAsync(x => x.Id == unProtectId);
        }

        /// <summary>
        /// 根据ID查询数据列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> QueryByIds(List<string> ids)
        {
            var idsUnProtect = ids.Select(x => DataProtector.Unprotect(x));
            return await Db.Queryable<T>().In(idsUnProtect).ToListAsync();
        }

        #endregion

        #region Insert
        /// <summary>
        /// 新增实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>返回自增量</returns>
        public virtual async Task<string> Insert(T entity)
        {
            var id = await Db.Insertable(entity).ExecuteReturnIdentityAsync();
            return DataProtector.Protect(id.ToString());
        }

        /// <summary>
        /// 新增实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="columns">指定只插入列</param>
        /// <returns>返回自增量</returns>
        public virtual async Task<string> Insert(T entity, Expression<Func<T, object>> columns)
        {
            var id = await Db.Insertable(entity).InsertColumns(columns).ExecuteReturnIdentityAsync();
            return DataProtector.Protect(id.ToString());
        }

        /// <summary>
        /// 新增实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="columns">指定只插入列</param>
        /// <returns>返回自增量</returns>
        public virtual async Task<string> Insert(T entity, params string[] columns)
        {
            var id = await Db.Insertable(entity).InsertColumns(columns).ExecuteReturnIdentityAsync();
            return DataProtector.Protect(id.ToString());
        }


        /// <summary>
        /// 批量插入实体(性能很快不用操心）
        /// </summary>
        /// <param name="listEntity"></param>
        /// <returns>受影响行数</returns>
        public virtual async Task<int> Insert(List<T> listEntity)
        {
            return await Db.Insertable(listEntity).ExecuteCommandAsync();
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="ignoreAllNullColumns">是NULL的列不更新</param>
        /// <param name="ignoreAllDefaultValue">默认值的列不更新</param>
        /// <returns></returns>
        public virtual async Task<bool> Update(T entity, bool ignoreAllNullColumns = true, bool ignoreAllDefaultValue = true)
        {
            entity.ModifyTime = DateTime.Now;
            return await Db.Updateable(entity).IgnoreColumns(x => x.CreateTime).IgnoreColumns(true, ignoreAllDefaultValue: true).ExecuteCommandHasChangeAsync();
        }
        /// <summary>
        ///  更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="ignoreExpression">不更新的列</param>
        /// <param name="updateExpression">更新的列</param>
        /// <returns></returns>
        public virtual async Task<bool> Update(T entity, Expression<Func<T, object>> ignoreExpression = null, Expression<Func<T, object>> updateExpression = null)
        {
            entity.ModifyTime = DateTime.Now;
            if (ignoreExpression != null)
            {
                return await Db.Updateable(entity).IgnoreColumns(ignoreExpression).ExecuteCommandHasChangeAsync();
            }
            else if (updateExpression != null)
            {
                return await Db.Updateable(entity).UpdateColumns(updateExpression).ExecuteCommandHasChangeAsync();
            }
            return await this.Update(entity, true);

        }
        /// <summary>
        ///  更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="ignoreColumns">不更新的列</param>
        /// <returns></returns>
        public virtual async Task<bool> Update(T entity, params string[] ignoreColumns)
        {
            entity.ModifyTime = DateTime.Now;
            return await Db.Updateable(entity).IgnoreColumns(ignoreColumns).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 批量更新(主键要有值，主键是更新条件)
        /// </summary>
        /// <param name="listEntity"></param>
        /// <returns></returns>
        public virtual async Task<bool> Update(List<T> listEntity)
        {
            return await Db.Updateable(listEntity).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 根据主键批量更新部分列
        /// </summary>
        /// <param name="ids">主键</param>
        /// <param name="updateExpression">部分列</param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateByIds(List<string> ids, Expression<Func<T, bool>> updateExpression)
        {
            var idsUnProtect = ids.Select(x => DataProtector.Unprotect(x));
            return await Db.Updateable<T>().SetColumns(updateExpression).Where(it => idsUnProtect.Contains(it.Id)).ExecuteCommandHasChangeAsync();
        }
        #endregion

        #region Delete
        /// <summary>
        /// 根据主键删除(假删除)
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteById(string id)
        {
            var unProtectId = DataProtector.Unprotect(id);
            return await Db.Updateable<T>().SetColumns(it => it.IsDeleted == 1).Where(it => it.Id == unProtectId).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 删除指定ID集合的数据(批量假删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteByIds(List<string> ids)
        {
            var list = await this.QueryByIds(ids);
            return await Db.Updateable(list).SetColumns(it => it.IsDeleted == 1).ExecuteCommandHasChangeAsync();
        }
        #endregion


    }
}