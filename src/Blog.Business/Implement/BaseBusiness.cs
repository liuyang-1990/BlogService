using Blog.Model;
using Blog.Repository;

namespace Blog.Business.Implement
{

    public class BaseBusiness<T> where T : BaseEntity, new()
    {
        protected IBaseRepository<T> BaseRepository;

        public virtual string GetPageList(int pageIndex, int pageSize)
        {
            return BaseRepository.GetPageList(pageIndex, pageSize);
        }

        public virtual string GetDetail(int id)
        {
            return BaseRepository.GetDetail(id);
        }

        public virtual bool Insert(T entity)
        {
            return BaseRepository.Insert(entity);
        }

        public virtual bool Update(T entity)
        {
            return BaseRepository.Update(entity);
        }

        public virtual bool Delete(int id)
        {
            return BaseRepository.Delete(id);
        }


    }
}