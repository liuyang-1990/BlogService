using Blog.Model.Db;
using Blog.Model.Settings;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Blog.Repository.Implement
{
    public class TagRepository : BaseRepository<TagInfo>, ITagRespoitory
    {
        public TagRepository(IOptions<DbSetting> settings) : base(settings)
        {
        }

        /// <inheritdoc cref="BaseRepository{T}" />
        public override async Task<bool> IsExist(TagInfo entity)
        {
            return await Context.Db.Queryable<TagInfo>().AnyAsync(x => x.TagName == entity.TagName && x.IsDeleted == 0);
        }
    }
}