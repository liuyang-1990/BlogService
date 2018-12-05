using Blog.Infrastructure;
using Blog.Model;
using Microsoft.Extensions.Options;

namespace Blog.Repository.Implement
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IOptions<DBSetting> settings) : base(settings)
        {
        }
    }
}