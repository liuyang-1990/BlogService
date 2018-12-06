using Blog.Model.Db;
using Blog.Model.Settings;
using Microsoft.Extensions.Options;

namespace Blog.Repository.Implement
{
    public class UserRepository : BaseRepository<UserInfo>, IUserRepository
    {
        public UserRepository(IOptions<DbSetting> settings) : base(settings)
        {
        }
    }
}