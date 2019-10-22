using Blog.Model;
using Blog.Model.Request;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Blog.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SqlSugar;

namespace Blog.Business.Implement
{

    public class BaseBusiness<T> where T : BaseEntity, new()
    {
        protected IBaseRepository<T> BaseRepository;

        public virtual async Task<JsonResultModel<T>> GetPageList(GridParams param, Expression<Func<T, bool>> expression, List<string> ignoreColumns = null)
        {
            return await BaseRepository.QueryByPage(param, expression);
        }

        public virtual async Task<T> GetDetail(string id, List<string> ignoreColumns = null)
        {
            return await BaseRepository.QueryById(id);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<string>> Insert(T entity)
        {
            var response = new ResultModel<string>();
            response.IsSuccess = (await BaseRepository.Insert(entity)).ObjToInt() > 0;
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
            response.IsSuccess = (await BaseRepository.Insert(entity)).ObjToInt() > 0;
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
            response.IsSuccess = await BaseRepository.DeleteById(id.ToString());
            response.Status = response.IsSuccess ? "0" : "1";
            return response;
        }
    }
}