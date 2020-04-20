using Blog.Infrastructure.DI;
using Blog.Model.Db;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Blog.Repository.Implement
{
    [Injector(typeof(IAccountRepository), Lifetime = ServiceLifetime.Scoped)]
    public class AccountRepository : BaseRepository<UserOAuthMapping>, IAccountRepository
    {
        public AccountRepository(ISqlSugarClient sqlSugarClient) : base(sqlSugarClient)
        {

        }
    }
}