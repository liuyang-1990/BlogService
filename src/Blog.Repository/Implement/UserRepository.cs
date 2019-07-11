using Blog.Model;
using Blog.Model.Db;
using Blog.Model.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Repository.Implement
{
    public class UserRepository : BaseRepository<UserInfo>, IUserRepository
    {
        public UserRepository(IOptions<DbSetting> settings) : base(settings)
        {

        }

        public async Task<bool> IsExist(UserInfo entity, UserAction userAction)
        {
            if (userAction == UserAction.Add)
            {
                return await Context.Db.Queryable<UserInfo>().AnyAsync(x => x.UserName == entity.UserName);
            }
            return await Context.Db.Queryable<UserInfo>().AnyAsync(x => x.UserName == entity.UserName && x.Id != entity.Id);
        }


        public async Task<UserInfo> GetUserByUserName(string userName, string password)
        {
            return await Context.Db.Queryable<UserInfo>().SingleAsync(x => x.UserName == userName && x.Password == password);
        }

        public async Task<bool> ChangePassword(UserInfo userInfo)
        {
            return await Context.Db.Updateable(userInfo).SetColumns(it => new UserInfo { Password = userInfo.Password }).ExecuteCommandHasChangeAsync();
        }

        public Task<bool> UpdateStatus(List<int> ids, int status)
        {
            return Context.Db.Updateable<UserInfo>().SetColumns(it => new UserInfo() { Status = status }).Where(it => ids.Contains(it.Id)).ExecuteCommandHasChangeAsync();
        }
    }
}

