using Blog.Infrastructure.DI;
using Blog.Model.Entities;
using Blog.Model.Request;
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
        /// <typeparam name="TResult">返回的对象</typeparam>
        /// <param name="ids">主键</param>
        /// <param name="selectExpression">查询某几列</param>
        /// <returns></returns>
        public async Task<List<TResult>> QueryByIds<TResult>(List<TPrimaryKey> ids, Expression<Func<TEntity, TResult>> selectExpression)
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
            Expression<Func<T1, T2, bool>> whereLambda)
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
        /// Updates an existing entity by primary key.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(TEntity entity)
        {
            return await Db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// Updates an existing entity by Non primary key columns.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="whereColumns">Non primary key columns of the entity</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>> whereColumns)
        {
            return await Db.Updateable(entity).WhereColumns(whereColumns).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// Updates some columns of an existing entity by primary key.
        /// </summary>
        /// <param name="updateColumns">columns of the entity to be updated</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(Expression<Func<TEntity, object>> updateColumns)
        {
            return await Db.Updateable<TEntity>().UpdateColumns(updateColumns).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// Updates some columns of an existing entity by primary keys.
        /// </summary>
        /// <param name="ids">Primary keys</param>
        /// <param name="updateColumns">columns of the entity to be updated</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(List<TPrimaryKey> ids, Expression<Func<TEntity, bool>> updateColumns)
        {
            return await Db.Updateable<TEntity>().SetColumns(updateColumns).Where(it => ids.Contains(it.Id)).ExecuteCommandHasChangeAsync();
        }

        #endregion

        #region Delete
        /// <summary>
        /// Deletes an entity by function.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Db.Deleteable<TEntity>().Where(predicate).ExecuteCommandHasChangeAsync();
        }
        #endregion

        #region Tran
        /// <summary>
        /// use tran
        /// </summary>
        /// <param name="action">action that need to used in tran</param>
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