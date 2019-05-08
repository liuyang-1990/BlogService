using Blog.Model;
using Blog.Model.Db;
using System.Threading.Tasks;

namespace Blog.Repository
{
    public interface IUserRepository : IBaseRepository<UserInfo>
    {
        Task<bool> IsExist(UserInfo entity, UserAction userAction);
        Task<UserInfo> GetUserByUserName(string userName, string password);

        Task<bool> ChangePassword(UserInfo userInfo);
    }
}