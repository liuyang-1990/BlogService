using Blog.Model;
using Blog.Model.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Repository
{
    public interface ICategoryRepository : IBaseRepository<CategoryInfo>
    {
        Task<bool> IsExist(CategoryInfo entity, UserAction userAction);

        Task<List<CategoryInfo>> GetAllCategory();
    }
}