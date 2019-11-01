using AspectCore.Injector;
using Blog.Model;
using Blog.Model.Db;

namespace Blog.Repository.Implement
{
    [Injector(typeof(IArticleContentRepository), LifeTime = Lifetime.Scoped)]
    public class ArticleContentRepository : BaseRepository<ArticleContent>, IArticleContentRepository
    {
    }
}
