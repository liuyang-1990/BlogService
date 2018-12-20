using Blog.Model.Db;
using System.Threading.Tasks;

namespace Blog.Repository
{
    public interface IUserRepository : IBaseRepository<UserInfo>
    {
        Task<UserInfo> GetUserByUserName(string userName, string password);
    }
}