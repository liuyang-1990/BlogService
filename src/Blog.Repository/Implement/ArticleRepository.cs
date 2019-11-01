using AspectCore.Injector;
using Blog.Model;
using Blog.Model.Db;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Repository.Implement
{
    [Injector(typeof(IArticleRepository), LifeTime = Lifetime.Scoped)]
    public class ArticleRepository : BaseRepository<ArticleInfo>, IArticleRepository
    {
        private readonly ILogger<ArticleRepository> _logger;
        public ArticleRepository(ILogger<ArticleRepository> logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// 插入文章
        /// </summary>
        /// <param name="article">文章基本信息</param>
        /// <param name="content">文章内容信息</param>
        /// <param name="tags">标签</param>
        /// <param name="categories">分类</param>
        /// <returns></returns>
        public async Task<bool> Insert(ArticleInfo article, ArticleContent content, List<string> tags, List<string> categories)
        {
            try
            {
                Db.Ado.BeginTran();
                var id = (await base.Insert(article)).ToString();
                content.ArticleId = id;
                await Db.Insertable(content).ExecuteCommandAsync();
                var articleTags = tags.Select(tagId => new ArticleTag()
                {
                    TagId = tagId,
                    ArticleId = id
                }).ToList();
                await Db.Insertable(articleTags).ExecuteCommandAsync();
                var articleCategories = categories.Select(categoryId => new ArticleCategory()
                {
                    ArticleId = id,
                    CategoryId = categoryId
                }).ToList();
                await Db.Insertable(articleCategories).ExecuteCommandAsync();
                Db.Ado.CommitTran();
                return true;
            }
            catch (Exception ex)
            {
                Db.Ado.RollbackTran();
                _logger.LogError(ex.Message);
                return false;
            }
        }


        /// <summary>
        ///  更新文章
        /// </summary>
        /// <param name="article">文章基本信息</param>
        /// <param name="content">文章内容信息</param>
        /// <param name="tags">标签信息</param>
        /// <param name="categories">分类信息</param>
        /// <returns></returns>
        public async Task<bool> Update(ArticleInfo article, ArticleContent content, List<string> tags, List<string> categories)
        {
            try
            {
                Db.Ado.BeginTran();
                await base.Update(article);
                content.ArticleId = article.Id;
                await Db.Updateable(content).WhereColumns(it => it.ArticleId).ExecuteCommandAsync();
                //先删除再添加
                await Db.Deleteable<ArticleTag>().Where(x => x.ArticleId == article.Id).ExecuteCommandAsync();
                await Db.Deleteable<ArticleCategory>().Where(x => x.ArticleId == article.Id).ExecuteCommandAsync();
                var articleTags = tags.Select(tagId => new ArticleTag()
                {
                    TagId = tagId,
                    ArticleId = article.Id,
                }).ToList();
                await Db.Insertable(articleTags).ExecuteCommandAsync();

                var articleCategories = categories.Select(categoryId => new ArticleCategory()
                {
                    ArticleId = article.Id,
                    CategoryId = categoryId
                }).ToList();
                await Db.Insertable(articleCategories).ExecuteCommandAsync();
                Db.Ado.CommitTran();
                return true;
            }
            catch (Exception ex)
            {
                Db.Ado.RollbackTran();
                _logger.LogError(ex.Message);
                return false;
            }
        }

      
    }
}