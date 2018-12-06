using Blog.Model;
using Blog.Model.ViewModel;
using Blog.Repository;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{

    public class BaseBusiness<T> where T : BaseEntity, new()
    {
        protected IBaseRepository<T> BaseRepository;

        public virtual async Task<JsonResultModel<T>> GetPageList(int pageIndex, int pageSize, Expression<Func<T, bool>> expression)
        {
            return await BaseRepository.GetPageList(pageIndex, pageSize, expression);
        }

        public virtual async Task<T> GetDetail(int id)
        {
            return await BaseRepository.GetDetail(id);
        }

        public virtual async Task<bool> Insert(T entity)
        {
            return await BaseRepository.Insert(entity);
        }

        public virtual async Task<bool> Update(T entity)
        {
            return await BaseRepository.Update(entity);
        }

        public virtual async Task<bool> Delete(int id)
        {
            return await BaseRepository.Delete(id);
        }
    }
}