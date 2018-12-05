using Blog.Model;
using Blog.Repository;

namespace Blog.Business.Implement
{

    public class BaseBusiness<T> where T : BaseEntity, new()
    {
        protected IBaseRepository<T> BaseRepository;

        public string GetPageList(int pageIndex, int pageSize)
        {
            return BaseRepository.GetPageList(pageIndex, pageSize);
        }

        public string GetDetail(int id)
        {
            return BaseRepository.GetDetail(id);
        }

        public bool Insert(T entity)
        {
            return BaseRepository.Insert(entity);
        }

        public bool Update(T entity)
        {
            return BaseRepository.Update(entity);
        }

        public bool Delete(int id)
        {
            return BaseRepository.Delete(id);
        }


    }
}