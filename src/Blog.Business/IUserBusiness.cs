using System.Threading.Tasks;
using Blog.Model.Db;

namespace Blog.Business
{
    public interface IUserBusiness: IBaseBusiness<UserInfo>
    {
        Task<UserInfo> GetUserByUserName(string userName, string password);
    }
}