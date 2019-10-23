using AspectCore.Injector;
using Blog.Model;
using Blog.Model.Db;

namespace Blog.Repository.Implement
{
    [Injector(typeof(ITagRepository), LifeTime = Lifetime.Scoped)]
    public class TagRepository : BaseRepository<TagInfo>, ITagRepository
    {
    }
}