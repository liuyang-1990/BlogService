using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.ViewModel;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface IUserBusiness : IBaseBusiness<UserInfo>
    {

        Task<JsonResultModel<UserInfo>> GetPageList(UserRequest searchParams, GridParams param);
        Task<UserInfo> GetUserByUserName(string userName, string password);
    }
}