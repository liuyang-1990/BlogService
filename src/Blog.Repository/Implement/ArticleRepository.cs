using AspectCore.Injector;
using Blog.Model;
using Blog.Model.Db;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Model.Request;
using SqlSugar;

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
        /// 获取文章详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ArticleDetailResponse> GetArticleDetail(string id)
        {
            var response = new ArticleDetailResponse();
            try
            {
                response.ArticleInfo = await Db.Queryable<ArticleInfo, ArticleContent>((ai, ac) => ai.Id == ac.Id)
                    .Where((ai, ac) => ai.Id == id && ai.IsDeleted == 0 && ai.Status == 1)
                    .Select((ai, ac) => new V_Article_Info()
                    {
                        Id = ai.Id,
                        Title = ai.Title,
                        Abstract = ai.Abstract,
                        ImageUrl = ai.ImageUrl,
                        Content = ac.Content,
                        Comments = ai.Comments,
                        Likes = ai.Likes,
                        Views = ai.Views,
                        CreateTime = ai.CreateTime
                    }).FirstAsync();
                var cid = await Db.Queryable<ArticleCategory>().Where(x => x.ArticleId == id).Select(x => x.CategoryId).ToListAsync();
                response.Categories = await Db.Queryable<CategoryInfo>().In(cid).Select(x => new Property()
                {
                    Key = x.Id,
                    Value = x.CategoryName
                }).ToListAsync();
                var tids = await Db.Queryable<ArticleTag>().Where(x => x.ArticleId == id).Select(x => x.TagId).ToListAsync();
                var tags = await Db.Queryable<TagInfo>().In(tids).OrderBy(x => x.CreateTime).Select(x => new Property()
                {
                    Key = x.Id,
                    Value = x.TagName
                }).ToListAsync();
                response.Tags = tids.Select(tid => tags.FirstOrDefault(x => x.Key == tid.ToString())).Where(tagInfo => tagInfo != null).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return response;
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
                var tagIds = new List<string>();
                foreach (var tag in tags)
                {
                    if (!int.TryParse(tag, out _))
                    {
                        //需要新增的tag
                        var tagId = await Db.Insertable(new TagInfo() { TagName = tag }).ExecuteReturnIdentityAsync();
                        tagIds.Add(tagId.ToString());
                    }
                    else
                    {
                        tagIds.Add(tag);
                    }
                }
                var articleTags = tagIds.Select(tagId => new ArticleTag()
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
                throw;
            }
        }


        /// <summary>
        ///  更新文章
        /// </summary>
        /// <param name="article"></param>
        /// <param name="content"></param>
        /// <param name="tags"></param>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        public async Task<bool> Update(ArticleInfo article, ArticleContent content, List<string> tags, List<string> categories)
        {
            try
            {
                Db.Ado.BeginTran();
                await Db.Updateable(article).IgnoreColumns(x => x.CreateTime).ExecuteCommandAsync();
                content.ArticleId = article.Id;
                await Db.Updateable(content).WhereColumns(it => it.ArticleId).ExecuteCommandAsync();
                //先删除再添加
                await Db.Deleteable<ArticleTag>().Where(x => x.ArticleId == article.Id).ExecuteCommandAsync();
                await Db.Deleteable<ArticleCategory>().Where(x => x.ArticleId == article.Id).ExecuteCommandAsync();
                var tagIds = new List<string>();
                foreach (var tag in tags)
                {
                    if (!int.TryParse(tag, out _))
                    {
                        //需要新增的tag
                        var tagId = await Db.Insertable(new TagInfo() { TagName = tag }).ExecuteReturnIdentityAsync();
                        tagIds.Add(tagId.ToString());
                    }
                    else
                    {
                        tagIds.Add(tag);
                    }
                }
                var articleTags = tagIds.Select(tagId => new ArticleTag()
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

        /// <summary>
        ///  根据分类获取文章
        /// </summary>
        /// <param name="categoryId">分类id</param>
        /// <param name="param">查询参数</param>
        /// <returns></returns>
        public async Task<JsonResultModel<ArticleInfo>> GetArticleByCategory(string categoryId, GridParams param)
        {
            RefAsync<int> total = 0;
            var articleIds = await Db.Queryable<ArticleCategory>().Where(i => i.CategoryId == categoryId)
                .GroupBy(x => x.ArticleId)
                .Select(x => x.ArticleId)
                .ToPageListAsync(param.PageNum, param.PageSize, total);
            return new JsonResultModel<ArticleInfo>()
            {
                Rows = await base.QueryByIds(articleIds),
                TotalRows = total
            };
        }

        /// <summary>
        /// 根据标签获取文章
        /// </summary>
        /// <param name="tagId">标签id</param>
        /// <param name="param">查询参数</param>
        /// <returns></returns>
        public async Task<JsonResultModel<ArticleInfo>> GetArticleByTag(string tagId, GridParams param)
        {
            RefAsync<int> total = 0;
            var articleIds = await Db.Queryable<ArticleTag>().Where(i => i.TagId == tagId)
                .GroupBy(x => x.ArticleId)
                .Select(x => x.ArticleId)
                .ToPageListAsync(param.PageNum, param.PageSize, total);
            return new JsonResultModel<ArticleInfo>()
            {
                Rows = await base.QueryByIds(articleIds),
                TotalRows = total
            };
        }
    }
}