using Blog.Model;
using Blog.Model.Request;
using Blog.Model.ViewModel;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Repository
{
    public interface IBaseRepository<T> where T : BaseEntity, new()
    {
        Task<JsonResultModel<T>> GetPageList(GridParams param, Expression<Func<T, bool>> expression);

        Task<T> GetDetail(int id);

        Task<bool> Insert(T entity);

        Task<bool> Update(T entity);

        Task<bool> Delete(int id);

    }
}