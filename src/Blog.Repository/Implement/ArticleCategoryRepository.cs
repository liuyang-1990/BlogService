using Blog.Infrastructure.DI;
using Blog.Model.Db;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Blog.Repository.Implement
{
    [Injector(typeof(IArticleCategoryRepository), Lifetime = ServiceLifetime.Scoped)]
    public class ArticleCategoryRepository : BaseRepository<ArticleCategory>, IArticleCategoryRepository
    {
        public ArticleCategoryRepository(ISqlSugarClient sqlSugarClient) : base(sqlSugarClient)
        {
        }
    }
}
