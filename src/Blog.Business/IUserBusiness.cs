using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface IUserBusiness : IBaseBusiness<UserInfo>
    {
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="searchParams"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<JsonResultModel<UserInfo>> GetPageList(UserRequest searchParams, GridParams param);

        Task<UserInfo> GetUserByUserName(string userName, string password);

        Task<ResultModel<string>> UpdatePassword(ChangePasswordRequest request);

        Task<ResultModel<string>> UpdateStatus(UpdateStatusRequest request);
    }
}