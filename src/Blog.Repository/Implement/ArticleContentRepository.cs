using Blog.Infrastructure.DI;
using Blog.Model.Db;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Blog.Repository.Implement
{
    [Injector(typeof(IArticleContentRepository), Lifetime = ServiceLifetime.Scoped)]
    public class ArticleContentRepository : BaseRepository<ArticleContent>, IArticleContentRepository
    {
        public ArticleContentRepository(ISqlSugarClient sqlSugarClient) : base(sqlSugarClient)
        {
        }
    }
}
