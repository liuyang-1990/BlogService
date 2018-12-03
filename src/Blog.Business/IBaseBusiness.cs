using System.Threading.Tasks;

namespace Blog.Business
{
    public interface IBaseBusiness<T> where T : class, new()
    {

        bool Insert(T entity);


    }
}