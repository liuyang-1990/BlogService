using AspectCore.Injector;
using Blog.Model;
using Blog.Model.Db;

namespace Blog.Repository.Implement
{
    [Injector(typeof(ICategoryRepository), LifeTime = Lifetime.Scoped)]
    public class CategoryRepository : BaseRepository<CategoryInfo>, ICategoryRepository
    {
    }
}