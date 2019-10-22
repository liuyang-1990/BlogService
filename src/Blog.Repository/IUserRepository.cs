using Blog.Model;
using Blog.Model.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Repository
{
    public interface IUserRepository : IBaseRepository<UserInfo>
    {

       // Task<bool> IsExist(UserInfo entity, UserAction userAction);

        /// <summary>
        /// 根据用户名和密码获取用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Task<UserInfo> GetUserByUserName(string userName, string password);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        Task<bool> ChangePassword(UserInfo userInfo);

        /// <summary>
        /// 批量启用或者禁用用户
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<bool> UpdateStatus(List<string> ids, int status);
    }
}