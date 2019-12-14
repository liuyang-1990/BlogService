using Blog.Infrastructure.DI;
using Blog.Model.Db;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Repository.Implement
{
    [Injector(typeof(IUserRepository), Lifetime = ServiceLifetime.Scoped)]
    public class UserRepository : BaseRepository<UserInfo>, IUserRepository
    {

        
    }
}

