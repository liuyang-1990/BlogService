using Blog.Infrastructure.DI;
using Blog.Model.Db;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Repository.Implement
{
    [Injector(typeof(IArticleCategoryRepository), Lifetime = ServiceLifetime.Scoped)]
    public class ArticleCategoryRepository : BaseRepository<ArticleCategory>, IArticleCategoryRepository
    {

    }
}
