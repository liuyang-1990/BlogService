using Blog.Model;
using Blog.Model.Request;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Blog.Repository;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{

    public class BaseBusiness<T> where T : BaseEntity, new()
    {
        protected IBaseRepository<T> BaseRepository;

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param">分页以及排序参数</param>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        public virtual async Task<JsonResultModel<T>> GetPageList(GridParams param, Expression<Func<T, bool>> expression)
        {
            return await BaseRepository.QueryByPage(param, expression);
        }
        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<T> GetDetail(string id)
        {
            return await BaseRepository.QueryById(id);
        }
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> QueryAll()
        {
            return await BaseRepository.QueryAll();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<string>> Insert(T entity)
        {
            var response = new ResultModel<string>();
            response.IsSuccess = await BaseRepository.Insert(entity) > 0;
            response.Status = response.IsSuccess ? "0" : "1";
            return response;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<string>> Update(T entity)
        {
            var response = new ResultModel<string>();
            response.IsSuccess = await BaseRepository.Insert(entity) > 0;
            response.Status = response.IsSuccess ? "0" : "1";
            return response;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<string>> Delete(string id)
        {
            var response = new ResultModel<string>();
            response.IsSuccess = await BaseRepository.DeleteById(id);
            response.Status = response.IsSuccess ? "0" : "1";
            return response;
        }
    }
}