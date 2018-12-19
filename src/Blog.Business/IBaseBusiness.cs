using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blog.Model;
using Blog.Model.Response;
using Blog.Model.ViewModel;

namespace Blog.Business
{
    public interface IBaseBusiness<T> where T : BaseEntity, new()
    {

        Task<JsonResultModel<T>> GetPageList(int pageIndex, int pageSize, Expression<Func<T, bool>> expression);

        Task<T> GetDetail(int id);

        Task<BaseResponse> Insert(T entity);

        Task<BaseResponse> Update(T entity);

        Task<BaseResponse> Delete(int id);


    }
}