using Blog.Infrastructure.DI;
using Blog.Model.Db;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Repository.Implement
{
    [Injector(typeof(ICategoryRepository), ServiceLifetime = ServiceLifetime.Scoped)]
    public class CategoryRepository : BaseRepository<CategoryInfo>, ICategoryRepository
    {
    }
}