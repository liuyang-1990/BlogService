using AspectCore.Injector;
using Blog.Model;
using Blog.Model.Db;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Repository.Implement
{
    [Injector(typeof(IArticleRepository), LifeTime = Lifetime.Scoped)]
    public class ArticleRepository : BaseRepository<ArticleInfo>, IArticleRepository
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ArticleDetailResponse> GetArticleDetail(int id)
        {
            var response = new ArticleDetailResponse();
            try
            {
                response.ArticleInfo = await Db.Queryable<V_Article_Info>().FirstAsync(x => x.Id == id);
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
                response.Tags = tids.Select(tid => tags.FirstOrDefault(x => x.Key == tid)).Where(tagInfo => tagInfo != null).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return response;
        }

        /// <summary>
        /// 插入文章
        /// </summary>
        /// <param name="article"></param>
        /// <param name="content"></param>
        /// <param name="tags"></param>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        public async Task<bool> Insert(ArticleInfo article, ArticleContent content, string[] tags, List<int> categoryIds)
        {
            try
            {
                Db.Ado.BeginTran();
                var id = await Db.Insertable(article).ExecuteReturnIdentityAsync();
                content.ArticleId = id;
                await Db.Insertable(content).ExecuteCommandAsync();
                var tagIds = new List<int>();
                foreach (var tag in tags)
                {
                    if (!Db.Queryable<TagInfo>().Any(i => i.TagName == tag))
                    {
                        //需要新增的tag
                        var tagId = await Db.Insertable(new TagInfo() { TagName = tag }).ExecuteReturnIdentityAsync();
                        tagIds.Add(tagId);
                    }
                    else
                    {
                        var tagInfo = await Db.Queryable<TagInfo>().FirstAsync(i => i.TagName == tag);
                        tagIds.Add(tagInfo.Id);
                    }
                }
                var articleTags = tagIds.Select(tagId => new ArticleTag()
                {
                    TagId = tagId,
                    ArticleId = id
                }).ToList();
                await Db.Insertable(articleTags).ExecuteCommandAsync();
                var articleCategories = categoryIds.Select(categoryId => new ArticleCategory()
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
                _logger.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 假删除文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<bool> Delete(int id)
        {
            try
            {
                Db.Ado.BeginTran();
                await Db.Updateable<ArticleInfo>().SetColumns(it => it.IsDeleted == 1).Where(it => it.Id == id).ExecuteCommandAsync();
                Db.Ado.CommitTran();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Db.Ado.RollbackTran();
                return false;
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
        public async Task<bool> Update(ArticleInfo article, ArticleContent content, string[] tags, List<int> categoryIds)
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
                var tagIds = new List<int>();
                foreach (var tag in tags)
                {
                    if (!Db.Queryable<TagInfo>().Any(i => i.TagName == tag))
                    {
                        //需要新增的tag
                        var tagId = await Db.Insertable(new TagInfo() { TagName = tag }).ExecuteReturnIdentityAsync();
                        tagIds.Add(tagId);
                    }
                    else
                    {
                        var tagInfo = await Db.Queryable<TagInfo>().FirstAsync(i => i.TagName == tag);
                        tagIds.Add(tagInfo.Id);
                    }
                }
                var articleTags = tagIds.Select(tagId => new ArticleTag()
                {
                    TagId = tagId,
                    ArticleId = article.Id,
                }).ToList();
                await Db.Insertable(articleTags).ExecuteCommandAsync();

                var articleCategories = categoryIds.Select(categoryId => new ArticleCategory()
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
                _logger.Error(ex.Message);
                return false;
            }
        }


        public async Task<List<ArticleInfo>> GetArticleByCategory(int categoryId, int pageIndex, int pageSize)
        {
            var articleIds = await Db.Queryable<ArticleCategory>().Where(i => i.CategoryId == categoryId)
                .GroupBy(x => x.ArticleId).Select(x => x.ArticleId).ToPageListAsync(pageIndex, pageSize);
            return await Db.Queryable<ArticleInfo>().In(articleIds).ToListAsync();
        }

        public async Task<List<ArticleInfo>> GetArticleByTag(int tagId, int pageIndex, int pageSize)
        {
            var articleIds = await Db.Queryable<ArticleTag>().Where(i => i.TagId == tagId)
                .GroupBy(x => x.ArticleId)
                .Select(x => x.ArticleId).ToPageListAsync(pageIndex, pageSize);
            return await Db.Queryable<ArticleInfo>().In(articleIds).ToListAsync();
        }
    }
}