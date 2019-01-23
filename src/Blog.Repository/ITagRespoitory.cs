using System.Threading.Tasks;
using Blog.Model;
using Blog.Model.Db;

namespace Blog.Repository
{
    public interface ITagRespoitory : IBaseRepository<TagInfo>
    {
        Task<bool> IsExist(TagInfo entity, UserAction userAction);
    }
}