using Blog.Model.Entities;
using Blog.Model.Request;
using Blog.Model.ViewModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Repository
{
    /// <summary>
    /// This interface is implemented by all repositories to ensure implementation of fixed methods.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IBaseRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>, new()
    {

        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns></returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> QueryAll();

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param">分页以及排序参数</param>
        /// <param name="whereExpression">条件</param>
        /// <returns></returns>
        Task<JsonResultModel<TEntity>> QueryByPage(GridParams param, Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        ///  分页查询
        /// </summary>
        /// <param name="param">分页以及排序参数</param>
        /// <param name="whereExpression">条件</param>
        /// <param name="groupByExpression">groupBy</param>
        /// <param name="selectExpression">select</param>
        /// <param name="totalCount">返回总条数</param>
        /// <returns></returns>
        Task<List<TResult>> QueryByPage<TResult>(GridParams param,
            Expression<Func<TEntity, bool>> whereExpression,
            Expression<Func<TEntity, object>> groupByExpression,
            Expression<Func<TEntity, TResult>> selectExpression,
            RefAsync<int> totalCount);

        /// <summary>
        /// Gets exactly one entity with given predicate.
        /// Throws exception if no entity or more than one entity.
        /// </summary>
        /// <param name="predicate">a filter</param>
        /// <returns>entity</returns>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<TEntity> SingleAsync(TPrimaryKey id);


        /// <summary>
        ///  根据ID查询数据列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryByIds(List<TPrimaryKey> ids);

        /// <summary>
        /// 根据ID查询数据列表
        /// </summary>
        /// <typeparam name="TResult">返回的对象</typeparam>
        /// <param name="ids">主键</param>
        /// <param name="selectExpression">查询某几列</param>
        /// <returns></returns>
        Task<List<TResult>> QueryByIds<TResult>(List<TPrimaryKey> ids, Expression<Func<TEntity, TResult>> selectExpression);
        /// <summary>
        ///  根据where条件查询某几列
        /// </summary>
        /// <param name="whereExpression">where条件</param>
        /// <param name="selectExpression">查询某几列</param>
        /// <returns></returns>
        Task<List<TResult>> QueryByWhere<TResult>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TResult>> selectExpression);

        /// <summary>
        /// 多表联合查询
        /// </summary>
        /// <typeparam name="T1">一表</typeparam>
        /// <typeparam name="T2">二表</typeparam>
        /// <typeparam name="T3">返回实体</typeparam>
        /// <param name="joinExpression"></param>
        /// <param name="selectExpression"></param>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        Task<T3> JoinQuery<T1, T2, T3>(
            Expression<Func<T1, T2, object[]>> joinExpression,
            Expression<Func<T1, T2, T3>> selectExpression,
            Expression<Func<T1, T2, bool>> whereLambda);


        /// <summary>
        /// 新增实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>返回自增量</returns>
        Task<int> Insert(TEntity entity);

        /// <summary>
        /// 批量插入实体(性能很快不用操心）
        /// </summary>
        /// <param name="listEntity"></param>
        /// <returns>受影响行数</returns>
        Task<int> Insert(List<TEntity> listEntity);

        /// <summary>
        ///  Updates an existing entity by primary key.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity);

        /// <summary>
        /// Updates an existing entity by Non primary key columns.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="whereColumns">Non primary key columns of the entity</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>> whereColumns);

        /// <summary>
        /// Updates some columns of an existing entity.
        /// </summary>
        /// <param name="updateColumns">columns of TEntity to be updated</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(Expression<Func<TEntity, object>> updateColumns);

        /// <summary>
        /// Updates some columns of an existing entity by primary keys.
        /// </summary>
        /// <param name="ids">Primary keys</param>
        /// <param name="updateColumns">columns of the entity to be updated</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(List<TPrimaryKey> ids, Expression<Func<TEntity, bool>> updateColumns);

        #region Delete
        /// <summary>
        /// Deletes an entity by function.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
        #endregion

        #region Tran
        /// <summary>
        /// use tran
        /// </summary>
        /// <param name="action">action that need to used in tran</param>
        /// <returns></returns>
        Task<DbResult<bool>> UseTranAsync(Action action);
        #endregion
    }
}