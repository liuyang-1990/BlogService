using Blog.Model.Db;
using Blog.Model.Settings;
using Microsoft.Extensions.Options;

namespace Blog.Repository.Implement
{
    public class TagRepository : BaseRepository<TagInfo>, ITagRespoitory
    {
        public TagRepository(IOptions<DbSetting> settings) : base(settings)
        {
        }

    }
}