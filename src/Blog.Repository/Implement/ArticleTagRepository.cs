﻿using Blog.Infrastructure.DI;
using Blog.Model.Db;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Blog.Repository.Implement
{
    [Injector(typeof(IArticleTagRepository), Lifetime = ServiceLifetime.Scoped)]
    public class ArticleTagRepository : BaseRepository<ArticleTag>, IArticleTagRepository
    {
        public ArticleTagRepository(ISqlSugarClient sqlSugarClient) : base(sqlSugarClient)
        {
        }
    }
}
