using Blog.Infrastructure.DI;
using Blog.Model.Entities;
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
    /// <summary>
    /// Base class for custom repositories of the application.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
    public class BaseRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>, new()
    {
        protected ISqlSugarClient Db;

        protected IDataProtector DataProtector;

        protected BaseRepository()
        {
            Db = CoreContainer.Current.GetService<ISqlSugarClient>();
            DataProtector = CoreContainer.Current.GetService<IDataProtectionProvider>().CreateProtector("protect_params");
        }

        #region Query

        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Db.Queryable<TEntity>().AnyAsync(predicate);
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public async Task<List<TEntity>> QueryAll()
        {
            return await Db.Queryable<TEntity>().ToListAsync();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param">分页以及排序参数</param>
        /// <param name="whereExpression">条件</param>
        /// <returns></returns>
        public async Task<JsonResultModel<TEntity>> QueryByPage(GridParams param, Expression<Func<TEntity, bool>> whereExpression)
        {
            RefAsync<int> totalCount = 0;
            var queryable = Db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression)
                .OrderByIF(!string.IsNullOrEmpty(param.SortField) && !string.IsNullOrEmpty(param.SortOrder),
                    param.SortField + " " + param.SortOrder);
            return new JsonResultModel<TEntity>()
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
        public async Task<List<TResult>> QueryByPage<TResult>(GridParams param,
            Expression<Func<TEntity, bool>> whereExpression,
            Expression<Func<TEntity, object>> groupByExpression,
            Expression<Func<TEntity, TResult>> selectExpression,
            RefAsync<int> totalCount)
        {
            return await Db.Queryable<TEntity>().Where(whereExpression)
                .GroupBy(groupByExpression)
                .Select(selectExpression)
                .ToPageListAsync(param.PageNum, param.PageSize, totalCount);
        }

        /// <summary>
        /// Gets exactly one entity with given predicate.
        /// Throws exception if no entity or more than one entity.
        /// </summary>
        /// <param name="predicate">a filter</param>
        /// <returns>entity</returns>
        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Db.Queryable<TEntity>().SingleAsync(predicate);
        }

        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<TEntity> SingleAsync(TPrimaryKey id)
        {
            return await SingleAsync(CreateEqualityExpressionForId(id));
        }

        /// <summary>
        /// 根据ID查询数据列表
        /// </summary>
        /// <param name="ids">主键</param>
        /// <returns></returns>
        public async Task<List<TEntity>> QueryByIds(List<TPrimaryKey> ids)
        {
            return await Db.Queryable<TEntity>().In(ids).ToListAsync();
        }

        /// <summary>
        /// 根据ID查询数据列表
        /// </summary>
        /// <typeparam name="T1">返回的对象</typeparam>
        /// <param name="ids">主键</param>
        /// <param name="selectExpression">查询某几列</param>
        /// <returns></returns>
        public async Task<List<T1>> QueryByIds<T1>(List<TPrimaryKey> ids, Expression<Func<TEntity, T1>> selectExpression) where T1 : Property
        {
            return await Db.Queryable<TEntity>().In(ids).Select(selectExpression).ToListAsync();
        }

        /// <summary>
        ///  根据where条件查询某几列
        /// </summary>
        /// <param name="whereExpression">where条件</param>
        /// <param name="selectExpression">查询某几列</param>
        /// <returns></returns>
        public async Task<List<TResult>> QueryByWhere<TResult>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TResult>> selectExpression)
        {
            return await Db.Queryable<TEntity>().Where(whereExpression).Select(selectExpression).ToListAsync();
        }

        #endregion

        #region JoinQuery

        public async Task<T3> JoinQuery<T1, T2, T3>(
            Expression<Func<T1, T2, object[]>> joinExpression,
            Expression<Func<T1, T2, T3>> selectExpression,
            Expression<Func<T1, T2, bool>> whereLambda) where T3 : IEntity<TPrimaryKey>
        {
            return await Db.Queryable(joinExpression)
                .Where(whereLambda)
                .Select(selectExpression)
                .FirstAsync();
        }

        #endregion

        #region Insert
        /// <summary>
        /// 新增实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>返回自增量</returns>
        public async Task<int> Insert(TEntity entity)
        {
            return await Db.Insertable(entity).ExecuteReturnIdentityAsync();
        }

        /// <summary>
        /// 批量插入实体(性能很快不用操心）
        /// </summary>
        /// <param name="listEntity"></param>
        /// <returns>受影响行数</returns>
        public async Task<int> Insert(List<TEntity> listEntity)
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
        public async Task<bool> Update(TEntity entity)
        {
            return await Db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新的列</param>
        /// <returns></returns>
        public async Task<bool> Update(TEntity entity, Expression<Func<TEntity, object>> updateColumns)
        {
            return await Db.Updateable(entity).UpdateColumns(updateColumns).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="updateObj">要更新的实体</param>
        /// <param name="whereColumns">更新的条件</param>
        /// <returns></returns>
        public async Task<bool> UpdateByWhere(TEntity updateObj, Expression<Func<TEntity, object>> whereColumns)
        {
            return await Db.Updateable(updateObj).WhereColumns(whereColumns).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 根据主键批量更新某一列
        /// </summary>
        /// <param name="ids">主键</param>
        /// <param name="updateExpression">某一列</param>
        /// <returns></returns>
        public async Task<bool> UpdateByIds(List<TPrimaryKey> ids, Expression<Func<TEntity, bool>> updateExpression)
        {
            return await Db.Updateable<TEntity>().SetColumns(updateExpression).Where(it => ids.Contains(it.Id)).ExecuteCommandHasChangeAsync();
        }

        #endregion

        #region Delete
        /// <summary>
        /// 根据主键删除(假删除)
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<bool> DeleteById(TPrimaryKey id)
        {
            //TODO IsDeleteD
            return await Db.Updateable<TEntity>().Where(it => SqlFunc.Equals(it.Id, id)).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 真删除
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByWhere(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await Db.Deleteable<TEntity>().Where(whereExpression).ExecuteCommandHasChangeAsync();
        }
        #endregion

        #region Tran
        /// <summary>
        /// 使用事务
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<DbResult<bool>> UseTranAsync(Action action)
        {
            return await Db.Ado.UseTranAsync(action);
        }

        #endregion

        protected Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var leftExpression = Expression.PropertyOrField(lambdaParam, "Id");

            var idValue = Convert.ChangeType(id, typeof(TPrimaryKey));

            Expression<Func<object>> closure = () => idValue;
            var rightExpression = Expression.Convert(closure.Body, leftExpression.Type);

            var lambdaBody = Expression.Equal(leftExpression, rightExpression);

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }
}