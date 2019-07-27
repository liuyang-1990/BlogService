﻿using AspectCore.Injector;
using Blog.Model;
using Blog.Model.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Repository.Implement
{
    [Injector(typeof(ITagRepository), LifeTime = Lifetime.Scoped)]
    public class TagRepository : BaseRepository<TagInfo>, ITagRepository
    {

        /// <inheritdoc cref="BaseRepository{T}" />
        public async Task<bool> IsExist(TagInfo entity, UserAction userAction)
        {
            if (userAction == UserAction.Add)
            {
                return await Db.Queryable<TagInfo>().AnyAsync(x => x.TagName == entity.TagName);
            }
            return await Db.Queryable<TagInfo>().AnyAsync(x => x.TagName == entity.TagName && x.Id != entity.Id);
        }

        public async Task<List<TagInfo>> GetAllTags()
        {
            return await Db.Queryable<TagInfo>().ToListAsync();
        }
    }
}