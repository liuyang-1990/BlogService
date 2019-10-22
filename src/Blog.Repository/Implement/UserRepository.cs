using AspectCore.Injector;
using Blog.Model;
using Blog.Model.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Repository.Implement
{
    [Injector(typeof(IUserRepository), LifeTime = Lifetime.Scoped)]
    public class UserRepository : BaseRepository<UserInfo>, IUserRepository
    {
        //public async Task<bool> IsExist(UserInfo entity, bool isAdd)
        //{
        //    if (userAction == UserAction.Add)
        //    {
        //        return await Db.Queryable<UserInfo>().AnyAsync(x => x.UserName == entity.UserName);
        //    }
        //    return await Db.Queryable<UserInfo>().AnyAsync(x => x.UserName == entity.UserName && x.Id != entity.Id);
        //}

        /// <summary>
        /// 根据用户名和密码获取用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task<UserInfo> GetUserByUserName(string userName, string password)
        {
            return await base.QueryByWhere(x => x.UserName == userName && x.Password == password);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public async Task<bool> ChangePassword(UserInfo userInfo)
        {
            return await base.Update(userInfo, updateExpression: it => it.Password);
        }

        /// <summary>
        /// 批量启用或者禁用用户
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public Task<bool> UpdateStatus(List<string> ids, int status)
        {
            return base.UpdateByIds(ids, it => it.Status == status);
        }
    }
}

