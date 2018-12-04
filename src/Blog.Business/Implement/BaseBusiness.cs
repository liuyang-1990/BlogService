using Blog.Model;
using Blog.Repository;

namespace Blog.Business.Implement
{

    public class BaseBusiness<T> where T : BaseEntity, new()
    {
        private readonly IBaseRepository<T> _baseRepository;

        public BaseBusiness(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public string GetPageList(int pageIndex, int pageSize)
        {
            return _baseRepository.GetPageList(pageIndex, pageSize);
        }

        public string GetDetail(int id)
        {
            return _baseRepository.GetDetail(id);
        }

        public bool Insert(T entity)
        {
            return _baseRepository.Insert(entity);
        }

        public bool Update(T entity)
        {
            return _baseRepository.Update(entity);
        }

        public bool Delete(int id)
        {
            return _baseRepository.Delete(id);
        }


    }
}