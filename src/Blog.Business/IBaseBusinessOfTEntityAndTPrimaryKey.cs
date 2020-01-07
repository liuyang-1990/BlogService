using Blog.Model.Entities;
using Blog.Model.Request;
using Blog.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface IBaseBusiness<TEntity, in TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>, IHasModificationTime, ISoftDelete, new()
    {
        /// <summary>
        /// Gets entities with given predicate,page & sort params.
        /// </summary>
        /// <param name="param">page & sort</param>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns></returns>
        Task<JsonResultModel<TEntity>> GetPageList(GridParams param, Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get All Entities
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> QueryAll();

        /// <summary>
        /// Gets exactly one entity with primary key
        /// Throws exception if no entity or more than one entity.
        /// </summary>
        /// <param name="id">primary key</param>
        /// <returns>entity</returns>
        Task<TEntity> SingleAsync(TPrimaryKey id);

        /// <summary>
        /// insert an entity 
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>primary key of the entity</returns>
        Task<bool> InsertAsync(TEntity entity);

        /// <summary>
        /// Updates an existing entity by primary key.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity);

        /// <summary>
        /// soft delete an entity by primary key.
        /// </summary>
        /// <param name="id">primary key</param>
        /// <returns></returns>
        Task<bool> SoftDeleteAsync(TPrimaryKey id);


    }
}