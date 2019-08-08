using Blog.Model;
using Blog.Model.Request;
using Blog.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Repository
{
    public interface IBaseRepository<T> where T : BaseEntity, new()
    {
        Task<JsonResultModel<T>> GetPageList(GridParams param, Expression<Func<T, bool>> expression, List<string> ignoreColumns = null);

        Task<T> GetDetail(int id, List<string> ignoreColumns = null);

        Task<bool> Insert(T entity);

        Task<bool> Update(T entity);

        Task<bool> Delete(int id);

    }
}