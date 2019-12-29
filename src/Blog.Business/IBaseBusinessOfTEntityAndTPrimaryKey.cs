using Blog.Model.Entities;
using Blog.Model.Request;
using Blog.Model.ViewModel;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface IBaseBusiness<TEntity, in TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>, new()
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param">分页以及排序参数</param>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        Task<JsonResultModel<TEntity>> GetPageList(GridParams param, Expression<Func<TEntity, bool>> expression);
        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<TEntity> SingleAsync(TPrimaryKey id);

        /// <summary>
        /// 新增实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> InsertAsync(TEntity entity);

        /// <summary>
        /// 更新实体数据
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