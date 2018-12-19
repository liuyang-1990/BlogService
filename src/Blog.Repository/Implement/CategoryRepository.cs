using System.Threading.Tasks;
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


        /// <inheritdoc cref="BaseRepository{T}" />
        public override async Task<bool> IsExist(CategoryInfo entity)
        {
            return await Context.Db.Queryable<CategoryInfo>().AnyAsync(x => x.CategoryName == entity.CategoryName && x.IsDeleted == 0);
        }
    }
}