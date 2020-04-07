using Blog.Infrastructure.DI;
using Blog.Model.Db;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Blog.Repository.Implement
{
    [Injector(typeof(IFriendLinkRepository), Lifetime = ServiceLifetime.Scoped)]
    public class FriendLinkRepository : BaseRepository<FriendLink>, IFriendLinkRepository
    {
        public FriendLinkRepository(ISqlSugarClient sqlSugarClient) : base(sqlSugarClient)
        {
        }
    }
}