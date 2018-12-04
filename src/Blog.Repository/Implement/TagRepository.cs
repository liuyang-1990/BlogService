using Blog.Infrastructure;
using Blog.Model;
using Microsoft.Extensions.Options;

namespace Blog.Repository.Implement
{
    public class TagRepository:BaseRepository<Tag>,ITagRespoitory
    {
        public TagRepository(IOptions<DBSetting> settings) : base(settings)
        {
        }

    }
}