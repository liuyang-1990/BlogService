using Blog.Infrastructure;
using Blog.Model;
using Microsoft.Extensions.Options;

namespace Blog.Repository.Implement
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IOptions<DBSetting> settings) : base(settings)
        {
        }
    }
}