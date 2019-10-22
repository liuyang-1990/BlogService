﻿using AspectCore.Injector;
using Blog.Model;
using Blog.Model.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Repository.Implement
{
    [Injector(typeof(ICategoryRepository), LifeTime = Lifetime.Scoped)]
    public class CategoryRepository : BaseRepository<CategoryInfo>, ICategoryRepository
    {

        /// <inheritdoc cref="BaseRepository{T}" />
        public async Task<bool> IsExist(CategoryInfo entity, UserAction userAction)
        {
            if (userAction == UserAction.Add)
            {
                return await Db.Queryable<CategoryInfo>().AnyAsync(x => x.CategoryName == entity.CategoryName);
            }
            return await Db.Queryable<CategoryInfo>().AnyAsync(x => x.CategoryName == entity.CategoryName && x.Id != entity.Id);
        }

        public async Task<List<CategoryInfo>> GetAllCategory()
        {
            return await base.QueryAll();
        }
    }
}