using Blog.Model.Db;
using Blog.Model.Response;
using Blog.Model.Settings;
using Blog.Model.ViewModel;
using Microsoft.Extensions.Options;
using NLog;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Repository.Implement
{
    public class ArticleRepository : BaseRepository<ArticleInfo>, IArticleRepository
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ArticleRepository(IOptions<DbSetting> settings) : base(settings)
        {

        }

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
                response.ArticleInfo = await Context.Db.Queryable<V_Article_Info>().FirstAsync(x => x.Id == id);
                var cid = await Context.Db.Queryable<ArticleCategory>().Where(x => x.ArticleId == id).Select(x => x.CategoryId).ToListAsync();
                response.Categories = await Context.Db.Queryable<CategoryInfo>().In(cid).Select(x => new Property()
                {
                    Key = x.Id,
                    Value = x.CategoryName
                }).ToListAsync();
                var tids = await Context.Db.Queryable<ArticleTag>().Where(x => x.ArticleId == id).Select(x => x.TagId).ToListAsync();
                var tags = await Context.Db.Queryable<TagInfo>().In(tids).OrderBy(x => x.CreateTime).Select(x => new Property()
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
                Context.Db.Ado.BeginTran();
                var id = await Context.Db.Insertable(article).ExecuteReturnIdentityAsync();
                content.ArticleId = id;
                await Context.Db.Insertable(content).ExecuteCommandAsync();
                var tagIds = new List<int>();
                foreach (var tag in tags)
                {
                    if (!Context.Db.Queryable<TagInfo>().Any(i => i.TagName == tag))
                    {
                        //需要新增的tag
                        var tagId = await Context.Db.Insertable(new TagInfo() { TagName = tag }).ExecuteReturnIdentityAsync();
                        tagIds.Add(tagId);
                    }
                    else
                    {
                        var tagInfo = await Context.Db.Queryable<TagInfo>().FirstAsync(i => i.TagName == tag);
                        tagIds.Add(tagInfo.Id);
                    }
                }
                var articleTags = tagIds.Select(tagId => new ArticleTag()
                {
                    TagId = tagId,
                    ArticleId = id
                }).ToList();
                await Context.Db.Insertable(articleTags).ExecuteCommandAsync();
                var articleCategories = categoryIds.Select(categoryId => new ArticleCategory()
                {
                    ArticleId = id,
                    CategoryId = categoryId.ObjToInt()
                }).ToList();
                await Context.Db.Insertable(articleCategories).ExecuteCommandAsync();
                Context.Db.Ado.CommitTran();
                return true;
            }
            catch (Exception ex)
            {
                Context.Db.Ado.RollbackTran();
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
                Context.Db.Ado.BeginTran();
                await Context.Db.Updateable<ArticleInfo>().UpdateColumns(it => it.IsDeleted == 1).Where(it => it.Id == id).ExecuteCommandAsync();
                //var content = new ArticleContent() { ArticleId = id, IsDeleted = 1, ModifyTime = DateTime.Now };
                //await Context.Db.Updateable(content).UpdateColumns(s => new { s.IsDeleted, s.ModifyTime, s.ArticleId }).WhereColumns(it => new { it.ArticleId }).ExecuteCommandAsync();
                Context.Db.Ado.CommitTran();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Context.Db.Ado.RollbackTran();
                return false;
            }

        }

        /// <summary>
        ///  更新文章
        /// </summary>
        /// <param name="article"></param>
        /// <param name="content"></param>
        /// <param name="tagIds"></param>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        public async Task<bool> Update(ArticleInfo article, ArticleContent content, string[] tagIds, List<int> categoryIds)
        {
            try
            {
                Context.Db.Ado.BeginTran();
                await Context.Db.Updateable(article).IgnoreColumns(x => x.CreateTime).ExecuteCommandAsync();
                content.ArticleId = article.Id;
                await Context.Db.Updateable(content).WhereColumns(it => it.ArticleId).ExecuteCommandAsync();
                //先删除再添加
                await Context.Db.Deleteable<ArticleTag>().Where(x => x.ArticleId == article.Id).ExecuteCommandAsync();
                var articleTags = tagIds.Select(tagId => new ArticleTag()
                {
                    TagId = tagId.ObjToInt(),
                    ArticleId = article.Id
                }).ToList();
                await Context.Db.Insertable(articleTags).ExecuteCommandAsync();

                await Context.Db.Deleteable<ArticleCategory>().Where(x => x.ArticleId == article.Id).ExecuteCommandAsync();
                var articleCategories = categoryIds.Select(categoryId => new ArticleCategory()
                {
                    ArticleId = article.Id,
                    CategoryId = categoryId.ObjToInt()
                }).ToList();
                await Context.Db.Insertable(articleCategories).ExecuteCommandAsync();
                Context.Db.Ado.CommitTran();
                return true;
            }
            catch (Exception ex)
            {
                Context.Db.Ado.RollbackTran();
                _logger.Error(ex.Message);
                throw;
            }
        }


        public async Task<List<ArticleInfo>> GetArticleByCategory(int categoryId, int pageIndex, int pageSize)
        {
            var articleIds = await Context.Db.Queryable<ArticleCategory>().Where(i => i.CategoryId == categoryId)
                .GroupBy(x => x.ArticleId).Select(x => x.ArticleId).ToPageListAsync(pageIndex, pageSize);
            return await Context.Db.Queryable<ArticleInfo>().In(articleIds).ToListAsync();
        }

        public async Task<List<ArticleInfo>> GetArticleByTag(int tagId, int pageIndex, int pageSize)
        {
            var articleIds = await Context.Db.Queryable<ArticleTag>().Where(i => i.TagId == tagId)
                .GroupBy(x => x.ArticleId)
                .Select(x => x.ArticleId).ToPageListAsync(pageIndex, pageSize);
            return await Context.Db.Queryable<ArticleInfo>().In(articleIds).ToListAsync();
        }
    }
}