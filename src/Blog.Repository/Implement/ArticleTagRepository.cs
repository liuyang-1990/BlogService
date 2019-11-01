using AspectCore.Injector;
using Blog.Model;
using Blog.Model.Db;

namespace Blog.Repository.Implement
{
    [Injector(typeof(IArticleTagRepository), LifeTime = Lifetime.Scoped)]
    public class ArticleTagRepository : BaseRepository<ArticleTag>, IArticleTagRepository
    {

    }
}
