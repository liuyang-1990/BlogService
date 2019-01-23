using System.Threading.Tasks;
using Blog.Model;
using Blog.Model.Db;

namespace Blog.Repository
{
    public interface ICategoryRepository : IBaseRepository<CategoryInfo>
    {
        Task<bool> IsExist(CategoryInfo entity, UserAction userAction);
    }
}