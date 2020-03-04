using Blog.Infrastructure.DI;
using Blog.Model.Db;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Blog.Repository.Implement
{
    [Injector(typeof(ICategoryRepository), Lifetime = ServiceLifetime.Scoped)]
    public class CategoryRepository : BaseRepository<CategoryInfo>, ICategoryRepository
    {
        public CategoryRepository(ISqlSugarClient sqlSugarClient) : base(sqlSugarClient)
        {
        }
    }
}