﻿using Blog.Model;
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
        public async Task<bool> IsExist(TagInfo entity, UserAction userAction)
        {
            if (userAction == UserAction.Add)
            {
                return await Context.Db.Queryable<TagInfo>().AnyAsync(x => x.TagName == entity.TagName);
            }
            return await Context.Db.Queryable<TagInfo>().AnyAsync(x => x.TagName == entity.TagName && x.Id != entity.Id);
        }
    }
}