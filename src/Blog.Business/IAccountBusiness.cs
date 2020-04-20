using System.Threading.Tasks;
using Blog.Model.Db;

namespace Blog.Business
{
    public interface IAccountBusiness : IBaseBusiness<UserOAuthMapping>
    {
        Task<UserInfo> Authorize(string code, string state);
    }
}