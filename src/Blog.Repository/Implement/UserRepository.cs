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
        public override async Task<bool> IsExist(UserInfo entity)
        {
            return await Context.Db.Queryable<UserInfo>().AnyAsync(x => x.UserName == entity.UserName && x.IsDeleted == 0);
        }


        public async Task<UserInfo> GetUserByUserName(string userName, string password)
        {
            return await Context.Db.Queryable<UserInfo>().SingleAsync(x => x.UserName == userName && x.Password == password);
        }
    }

}