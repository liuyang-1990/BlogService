using Blog.Model.Entities;
using Blog.Model.Request;
using Blog.Model.ViewModel;
using Blog.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{

    public class BaseBusiness<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>, IHasModificationTime, ISoftDelete, new()
    {
        protected IBaseRepository<TEntity, TPrimaryKey> BaseRepository;

        /// <summary>
        /// Gets entities with given predicate,page & sort params.
        /// </summary>
        /// <param name="param">page & sort</param>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns></returns>
        public async Task<JsonResultModel<TEntity>> GetPageList(GridParams param, Expression<Func<TEntity, bool>> predicate)
        {
            return await BaseRepository.Query(param, predicate);
        }

        /// <summary>
        /// Gets exactly one entity with primary key
        /// Throws exception if no entity or more than one entity.
        /// </summary>
        /// <param name="id">primary key</param>
        /// <returns>entity</returns>
        public async Task<TEntity> SingleAsync(TPrimaryKey id)
        {
            return await BaseRepository.SingleAsync(id);
        }

        /// <summary>
        /// Get All Entities
        /// </summary>
        /// <returns></returns>
        public async Task<List<TEntity>> QueryAll()
        {
            return await BaseRepository.QueryAll();
        }

        /// <summary>
        /// insert an entity 
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>primary key of the entity</returns>
        public virtual async Task<bool> InsertAsync(TEntity entity)
        {
            return await BaseRepository.InsertAsync(entity) > 0;
        }

        /// <summary>
        /// Updates an existing entity by primary key.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            entity.ModifyTime = DateTime.Now;
            return await BaseRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// soft delete an entity by primary key.
        /// </summary>
        /// <param name="id">primary key</param>
        /// <returns></returns>
        public async Task<bool> SoftDeleteAsync(TPrimaryKey id)
        {
            return await BaseRepository.UpdateAsync(id, it => new TEntity { IsDeleted = true });
        }
    }
}