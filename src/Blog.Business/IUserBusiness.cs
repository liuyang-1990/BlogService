using System.Threading.Tasks;
using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.ViewModel;

namespace Blog.Business
{
    public interface IUserBusiness: IBaseBusiness<UserInfo>
    {

        Task<JsonResultModel<UserInfo>> GetPageList(int pageIndex, int pageSize, UserRequest userInfo);
        Task<UserInfo> GetUserByUserName(string userName, string password);
    }
}