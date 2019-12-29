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

    public class BaseBusiness<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>, new()
    {
        protected IBaseRepository<TEntity, TPrimaryKey> BaseRepository;

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param">分页以及排序参数</param>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public virtual async Task<JsonResultModel<TEntity>> GetPageList(GridParams param, Expression<Func<TEntity, bool>> predicate)
        {
            return await BaseRepository.Query(param, predicate);
        }
        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<TEntity> SingleAsync(TPrimaryKey id)
        {
            return await BaseRepository.SingleAsync(id);
        }
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> QueryAll()
        {
            return await BaseRepository.QueryAll();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<bool> InsertAsync(TEntity entity)
        {
            return await BaseRepository.InsertAsync(entity) > 0;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            return await BaseRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// soft delete an entity by primary key.
        /// </summary>
        /// <param name="id">primary key</param>
        /// <returns></returns>
        public virtual async Task<bool> SoftDeleteAsync(TPrimaryKey id)
        {
            return await BaseRepository.UpdateAsync(it => new { IsDeleted = 1 });
        }
    }
}