using Blog.Infrastructure.DI;
using Blog.Model.Db;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Repository.Implement
{
    [Injector(typeof(ITagRepository), ServiceLifetime = ServiceLifetime.Scoped)]
    public class TagRepository : BaseRepository<TagInfo>, ITagRepository
    {
    }
}