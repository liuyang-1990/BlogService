using Blog.Model;
using Blog.Model.Db;
using Blog.Model.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Repository.Implement
{
    public class CategoryRepository : BaseRepository<CategoryInfo>, ICategoryRepository
    {
        public CategoryRepository(IOptions<DbSetting> settings) : base(settings)
        {
        }


        /// <inheritdoc cref="BaseRepository{T}" />
        public async Task<bool> IsExist(CategoryInfo entity, UserAction userAction)
        {
            if (userAction == UserAction.Add)
            {
                return await Context.Db.Queryable<CategoryInfo>().AnyAsync(x => x.CategoryName == entity.CategoryName);
            }
            return await Context.Db.Queryable<CategoryInfo>().AnyAsync(x => x.CategoryName == entity.CategoryName && x.Id != entity.Id);
        }

        public async Task<List<CategoryInfo>> GetAllCategory()
        {
            return await Context.Db.Queryable<CategoryInfo>().ToListAsync();
        }
    }
}