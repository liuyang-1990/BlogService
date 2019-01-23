using Blog.Model;
using Blog.Model.Db;
using Blog.Model.Settings;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Blog.Repository.Implement
{
    public class UserRepository : BaseRepository<UserInfo>, IUserRepository
    {
        public UserRepository(IOptions<DbSetting> settings) : base(settings)
        {

        }

        /// <inheritdoc cref="BaseRepository{T}" />
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
    }

}