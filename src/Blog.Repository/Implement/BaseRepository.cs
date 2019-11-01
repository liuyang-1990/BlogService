using Blog.Infrastructure;
using Blog.Infrastructure.Extensions;
using Blog.Model;
using Blog.Model.Request;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Microsoft.AspNetCore.DataProtection;
using SqlSugar;
using System;
using System.Collections.Generic;
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
            DataProtector = AspectCoreContainer.Resolve<IDataProtectionProvider>().CreateProtector("protect_params");
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
            return await Db.Queryable<T>().Mapper(it =>
            {
                it.Id = DataProtector.Protect(it.Id);

            }).ToListAsync();
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
                .Mapper(it =>
                {
                    it.Id = DataProtector.Protect(it.Id);
                });
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
        /// <param name="whereExpression">条件</param>
        /// <param name="groupByExpression">groupBy</param>
        /// <param name="selectExpression">select</param>
        /// <param name="totalCount">返回总条数</param>
        /// <returns></returns>
        public virtual async Task<List<object>> QueryByPage(GridParams param,
            Expression<Func<T, bool>> whereExpression,
            Expression<Func<T, object>> groupByExpression,
            Expression<Func<T, object>> selectExpression, RefAsync<int> totalCount)
        {
            return await Db.Queryable<T>().Where(whereExpression)
                .GroupBy(groupByExpression)
                .Select(selectExpression)
                .ToPageListAsync(param.PageNum, param.PageSize, totalCount);
        }
        /// <summary>
        /// 根据where条件查询一条数据
        /// </summary>
        /// <param name="whereExpression">where条件</param>
        /// <returns></returns>
        public virtual async Task<T> QueryByWhere(Expression<Func<T, bool>> whereExpression)
        {
            return await Db.Queryable<T>().Mapper(it =>
            {
                it.Id = DataProtector.Protect(it.Id);

            }).FirstAsync(whereExpression);
        }

        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<T> QueryById(string id)
        {
            return await Db.Queryable<T>().Mapper(it =>
            {
                it.Id = DataProtector.Protect(it.Id);

            }).FirstAsync(x => x.Id == id);
        }

        /// <summary>
        /// 根据ID查询数据列表
        /// </summary>
        /// <param name="ids">主键</param>
        /// <returns></returns>
        public virtual async Task<List<T>> QueryByIds(List<string> ids)
        {
            return await Db.Queryable<T>().In(ids).Mapper(it =>
            {
                it.Id = DataProtector.Protect(it.Id);

            }).ToListAsync();
        }

        /// <summary>
        /// 根据ID查询数据列表
        /// </summary>
        /// <param name="ids">主键</param>
        /// <returns></returns>
        public virtual async Task<List<T>> QueryByIds(List<object> ids)
        {
            return await Db.Queryable<T>().In(ids).Mapper(it =>
            {
                it.Id = DataProtector.Protect(it.Id);

            }).ToListAsync();
        }
        /// <summary>
        /// 根据ID查询数据列表
        /// </summary>
        /// <typeparam name="T1">返回的对象</typeparam>
        /// <param name="ids">主键</param>
        /// <param name="selectExpression">查询某几列</param>
        /// <returns></returns>
        public virtual async Task<List<T1>> QueryByIds<T1>(List<object> ids, Expression<Func<T, T1>> selectExpression) where T1 : Property
        {
            return await Db.Queryable<T>().In(ids).Select(selectExpression).Mapper(it =>
            {
                it.Id = DataProtector.Protect(it.Id);

            }).ToListAsync();
        }

        /// <summary>
        ///  根据where条件查询某几列
        /// </summary>
        /// <param name="whereExpression">where条件</param>
        /// <param name="selectExpression">查询某几列</param>
        /// <returns></returns>
        public virtual async Task<List<object>> QueryByWhere(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> selectExpression)
        {
            return await Db.Queryable<T>().Where(whereExpression).Select(selectExpression).ToListAsync();
        }

        #endregion

        #region JoinQuery

        public virtual async Task<T3> JoinQuery<T1, T2, T3>(
            Expression<Func<T1, T2, object[]>> joinExpression,
            Expression<Func<T1, T2, T3>> selectExpression,
            Expression<Func<T1, T2, bool>> whereLambda) where T3 : IEntity
        {
            return await Db.Queryable(joinExpression)
                .Where(whereLambda)
                .Select(selectExpression)
                .Mapper(it =>
                {
                    it.Id = DataProtector.Protect(it.Id);

                }).FirstAsync();
        }

        #endregion

        #region Insert
        /// <summary>
        /// 新增实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>返回自增量</returns>
        public virtual async Task<int> Insert(T entity)
        {
            return await Db.Insertable(entity).ExecuteReturnIdentityAsync();
        }

        /// <summary>
        /// 新增实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>返回自增量</returns>
        public virtual async Task<int> Insert(T entity, Expression<Func<T, object>> insertColumns)
        {
            return await Db.Insertable(entity).InsertColumns(insertColumns).ExecuteReturnIdentityAsync();
        }

        /// <summary>
        /// 新增实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>返回自增量</returns>
        public virtual async Task<int> Insert(T entity, params string[] insertColumns)
        {
            return await Db.Insertable(entity).InsertColumns(insertColumns).ExecuteReturnIdentityAsync();
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
        ///  更新实体数据(以主键为条件)
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public virtual async Task<bool> Update(T entity)
        {
            entity.ModifyTime = DateTime.Now;
            return await Db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="ignoreAllDefaultAndNullValue">是NULL的列和默认值的列不更新</param>
        /// <returns></returns>
        public virtual async Task<bool> Update(T entity, bool ignoreAllDefaultAndNullValue)
        {
            entity.ModifyTime = DateTime.Now;
            return await Db.Updateable(entity).IgnoreColumns(ignoreAllDefaultAndNullValue, ignoreAllDefaultValue: ignoreAllDefaultAndNullValue).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新的列</param>
        /// <returns></returns>
        public virtual async Task<bool> Update(T entity, Expression<Func<T, object>> updateColumns)
        {
            entity.ModifyTime = DateTime.Now;
            return await Db.Updateable(entity).UpdateColumns(updateColumns).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 更新实体数据
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
        /// 根据主键批量更新某一列
        /// </summary>
        /// <param name="ids">主键</param>
        /// <param name="updateExpression">某一列</param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateByIds(List<string> ids, Expression<Func<T, bool>> updateExpression)
        {
            return await Db.Updateable<T>().SetColumns(updateExpression).Where(it => ids.Contains(it.Id)).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 根据主键批量更新部分列
        /// </summary>
        /// <param name="ids">主键</param>
        /// <param name="updateExpression">部分列</param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateByIds(List<string> ids, Expression<Func<T, T>> updateExpression)
        {
            return await Db.Updateable<T>().SetColumns(updateExpression).Where(it => ids.Contains(it.Id)).ExecuteCommandHasChangeAsync();
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
            return await Db.Updateable<T>().SetColumns(it => it.IsDeleted == 1).Where(it => it.Id == id).ExecuteCommandHasChangeAsync();
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