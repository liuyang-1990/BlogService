using Blog.Model.Common;
using Blog.Model.Db;
using Blog.Model.Request.User;
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

        /// <summary>
        /// 根据用户名和密码获取某个用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Task<UserInfo> GetUserByUserName(string userName, string password);

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> UpdatePassword(ChangePasswordRequest request);

        /// <summary>
        /// 更新状态 启用或禁用
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> UpdateStatus(UpdateStatusRequest request);
    }
}