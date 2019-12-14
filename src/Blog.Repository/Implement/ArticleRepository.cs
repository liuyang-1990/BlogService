using Blog.Infrastructure.DI;
using Blog.Model.Db;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Repository.Implement
{
    [Injector(typeof(IArticleRepository), Lifetime = ServiceLifetime.Scoped)]
    public class ArticleRepository : BaseRepository<ArticleInfo>, IArticleRepository
    {
    }
}