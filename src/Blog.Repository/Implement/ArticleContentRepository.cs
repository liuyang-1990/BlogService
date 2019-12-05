using Blog.Infrastructure.DI;
using Blog.Model.Db;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Repository.Implement
{
    [Injector(typeof(IArticleContentRepository), ServiceLifetime = ServiceLifetime.Scoped)]
    public class ArticleContentRepository : BaseRepository<ArticleContent>, IArticleContentRepository
    {
    }
}
