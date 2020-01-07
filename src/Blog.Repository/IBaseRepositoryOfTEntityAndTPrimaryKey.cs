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

        #region Query/Get
        /// <summary>
        ///  Query for existence with given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns></returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get All Entities
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> QueryAll();

        /// <summary>
        /// Gets entities with given predicate,page & sort params.
        /// </summary>
        /// <param name="param">page & sort</param>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns></returns>
        Task<JsonResultModel<TEntity>> Query(GridParams param, Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets entities with given predicate,page & sort params.
        /// </summary>
        /// <param name="param">page & sort</param>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="groupBy">groupBy</param>
        /// <param name="select">columns to be selected</param>
        /// <param name="totalCount">total count</param>
        /// <returns></returns>
        Task<List<TResult>> Query<TResult>(GridParams param,
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> groupBy,
            Expression<Func<TEntity, TResult>> select,
            RefAsync<int> totalCount);

        /// <summary>
        /// Gets exactly one entity with given predicate.
        /// Throws exception if no entity or more than one entity.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns>entity</returns>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets exactly one entity with primary key
        /// Throws exception if no entity or more than one entity.
        /// </summary>
        /// <param name="id">primary key</param>
        /// <returns>entity</returns>
        Task<TEntity> SingleAsync(TPrimaryKey id);

        /// <summary>
        /// get entities with given primary keys
        /// </summary>
        /// <param name="ids">primary keys</param>
        /// <returns></returns>
        Task<List<TEntity>> Query(List<TPrimaryKey> ids);

        /// <summary>
        /// get some columns of entities with given primary keys
        /// </summary>
        /// <typeparam name="TResult">the return entity</typeparam>
        /// <param name="ids">primary keys</param>
        /// <param name="select">columns to be selected</param>
        /// <returns></returns>
        Task<List<TResult>> Query<TResult>(List<TPrimaryKey> ids, Expression<Func<TEntity, TResult>> select);

        /// <summary>
        /// get some columns with given predicate
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="select">columns to be selected</param>
        /// <returns></returns>
        Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> select);
        #endregion

        #region Join Query
        /// <summary>
        /// Multi-table join query
        /// </summary>
        /// <typeparam name="TEntity1">Entity1</typeparam>
        /// <typeparam name="TEntity2">Entity2</typeparam>
        /// <typeparam name="TResult">return result</typeparam>
        /// <param name="join">a condition to join</param>
        /// <param name="select">columns of the TResult</param>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns>Entity</returns>
        Task<TResult> JoinQuery<TEntity1, TEntity2, TResult>(
            Expression<Func<TEntity1, TEntity2, object[]>> join,
            Expression<Func<TEntity1, TEntity2, TResult>> select,
            Expression<Func<TEntity1, TEntity2, bool>> predicate);

        #endregion

        #region Insert
        /// <summary>
        /// insert an entity 
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>primary key of the entity</returns>
        Task<int> InsertAsync(TEntity entity);

        /// <summary>
        /// insert entities(Fast performance)
        /// </summary>
        /// <param name="listEntity">Entities</param>
        /// <returns></returns>
        Task<int> InsertAsync(List<TEntity> listEntity);

        #endregion

        #region Update
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
        /// Updates some columns of an existing entity by primary key.
        /// </summary>
        /// <param name="id">primary key</param>
        /// <param name="updateColumns">columns of TEntity to be updated</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TPrimaryKey id, Expression<Func<TEntity, object>> updateColumns);

        /// <summary>
        /// Updates a column of an existing entity by primary keys.
        /// </summary>
        /// <param name="ids">Primary keys</param>
        /// <param name="updateColumns">a column of the entity to be updated</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(List<TPrimaryKey> ids, Expression<Func<TEntity, bool>> updateColumns);
        #endregion

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