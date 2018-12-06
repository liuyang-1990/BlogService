using Blog.Model.Db;
using Blog.Model.Settings;
using Microsoft.Extensions.Options;

namespace Blog.Repository.Implement
{
    public class CategoryRepository : BaseRepository<CategoryInfo>, ICategoryRepository
    {
        public CategoryRepository(IOptions<DbSetting> settings) : base(settings)
        {
        }
    }
}