using Blog.Model;
using Blog.Model.Request;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface IBaseBusiness<T> where T : BaseEntity, new()
    {

        Task<JsonResultModel<T>> GetPageList(GridParams param, Expression<Func<T, bool>> expression, List<string> ignoreColumns = null);

        Task<T> GetDetail(int id, List<string> ignoreColumns = null);

        Task<ResultModel<string>> Insert(T entity);

        Task<ResultModel<string>> Update(T entity);

        Task<ResultModel<string>> Delete(int id);


    }
}