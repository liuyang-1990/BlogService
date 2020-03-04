using Blog.Model.Common;
using Blog.Model.Db;
using Blog.Model.Request.User;
using Blog.Model.Response;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface IUserBusiness : IBaseBusiness<UserInfo>
    {
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<JsonResultModel<UserInfo>> GetPageList(UserSearchRequest request);

        Task<UserInfo> GetUserByUserName(string userName, string password);

        Task<ResultModel<string>> UpdatePassword(ChangePasswordRequest request);

        Task<ResultModel<string>> UpdateStatus(UpdateStatusRequest request);
    }
}