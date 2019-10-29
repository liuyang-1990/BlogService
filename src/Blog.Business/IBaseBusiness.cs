using Blog.Model;
using Blog.Model.Request;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface IBaseBusiness<T> where T : BaseEntity, new()
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param">分页以及排序参数</param>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        Task<JsonResultModel<T>> GetPageList(GridParams param, Expression<Func<T, bool>> expression);
        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<T> GetDetail(string id);
        /// <summary>
        /// 新增实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<ResultModel<string>> Insert(T entity);
        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<ResultModel<string>> Update(T entity);
        /// <summary>
        /// 根据主键删除(假删除)
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<ResultModel<string>> Delete(string id);


    }
}