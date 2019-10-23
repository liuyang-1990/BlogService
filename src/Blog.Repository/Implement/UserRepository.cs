using AspectCore.Injector;
using Blog.Model;
using Blog.Model.Db;

namespace Blog.Repository.Implement
{
    [Injector(typeof(IUserRepository), LifeTime = Lifetime.Scoped)]
    public class UserRepository : BaseRepository<UserInfo>, IUserRepository
    {

        
    }
}

