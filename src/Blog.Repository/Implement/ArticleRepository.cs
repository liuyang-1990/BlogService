using Blog.Infrastructure.DI;
using Blog.Model.Db;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Blog.Repository.Implement
{
    [Injector(typeof(IArticleRepository), Lifetime = ServiceLifetime.Scoped)]
    public class ArticleRepository : BaseRepository<ArticleInfo>, IArticleRepository
    {
        public ArticleRepository(ISqlSugarClient sqlSugarClient) : base(sqlSugarClient)
        {
        }
    }
}