using Blog.Model;
using Blog.Model.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Repository
{
    public interface ITagRespoitory : IBaseRepository<TagInfo>
    {
        Task<bool> IsExist(TagInfo entity, UserAction userAction);

        Task<IEnumerable<TagInfo>> GetAllTags();
    }
}