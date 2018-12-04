using Blog.Model;

namespace Blog.Repository
{
    public interface IBaseRepository<in T> where T : BaseEntity, new()
    {
        string GetPageList(int pageIndex, int pageSize);

        string GetDetail(int id);

        bool Insert(T entity);

        bool Update(T entity);

        bool Delete(int id);

    }
}