using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blog.Model;
using Blog.Model.ViewModel;

namespace Blog.Business
{
    public interface IBaseBusiness<T> where T : BaseEntity, new()
    {

        Task<JsonResultModel<T>> GetPageList(int pageIndex, int pageSize, Expression<Func<T, bool>> expression);

        Task<T> GetDetail(int id);

        Task<bool> Insert(T entity);

        Task<bool> Update(T entity);

        Task<bool> Delete(int id);


    }
}