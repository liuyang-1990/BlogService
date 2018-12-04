
namespace Blog.Business
{
    public interface IBaseBusiness<T> where T : class, new()
    {

        string GetPageList(int pageIndex, int pageSize);

        string GetDetail(int id);

        bool Insert(T entity);

        bool Update(T entity);

        bool Delete(int id);


    }
}