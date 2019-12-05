﻿using Blog.Infrastructure.DI;
using Blog.Model.Db;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Repository.Implement
{
    [Injector(typeof(IArticleTagRepository), ServiceLifetime = ServiceLifetime.Scoped)]
    public class ArticleTagRepository : BaseRepository<ArticleTag>, IArticleTagRepository
    {

    }
}
