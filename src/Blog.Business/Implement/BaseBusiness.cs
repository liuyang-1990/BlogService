using Blog.Model;
using Blog.Model.Request;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Blog.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{

    public class BaseBusiness<T> where T : BaseEntity, new()
    {
        protected IBaseRepository<T> BaseRepository;

        public virtual async Task<JsonResultModel<T>> GetPageList(GridParams param, Expression<Func<T, bool>> expression, List<string> ignoreColumns = null)
        {
            return await BaseRepository.GetPageList(param, expression, ignoreColumns);
        }

        public virtual async Task<T> GetDetail(int id, List<string> ignoreColumns = null)
        {
            return await BaseRepository.GetDetail(id, ignoreColumns);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<string>> Insert(T entity)
        {
            var response = new ResultModel<string>();
            response.IsSuccess = await BaseRepository.Insert(entity);
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
            response.IsSuccess = await BaseRepository.Update(entity);
            response.Status = response.IsSuccess ? "0" : "1";
            return response;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<string>> Delete(int id)
        {
            var response = new ResultModel<string>();
            response.IsSuccess = await BaseRepository.Delete(id);
            response.Status = response.IsSuccess ? "0" : "1";
            return response;
        }
    }
}