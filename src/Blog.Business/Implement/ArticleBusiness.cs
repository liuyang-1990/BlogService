using Blog.Infrastructure.DI;
using Blog.Model.Common;
using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.Request.Article;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Blog.Repository;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{
    [Injector(typeof(IArticleBusiness), Lifetime = ServiceLifetime.Scoped)]
    public class ArticleBusiness : BaseBusiness<ArticleInfo>, IArticleBusiness
    {

        private readonly IArticleRepository _articleRepository;
        private readonly IArticleContentRepository _articleContentRepository;
        private readonly IArticleTagRepository _articleTagRepository;
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ArticleBusiness(
            IArticleRepository articleRepository,
            IArticleContentRepository articleContentRepository,
            IArticleTagRepository articleTagRepository,
            IArticleCategoryRepository articleCategoryRepository,
            ITagRepository tagRepository,
            ICategoryRepository categoryRepository
            )
        {
            BaseRepository = articleRepository;
            _articleRepository = articleRepository;
            _articleContentRepository = articleContentRepository;
            _articleTagRepository = articleTagRepository;
            _articleCategoryRepository = articleCategoryRepository;
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<JsonResultModel<ArticleInfo>> GetPageList(ArticleSearchRequest request)
        {
            var exp = Expressionable.Create<ArticleInfo>().AndIF(request.IsPublished.HasValue, it => it.IsPublished == request.IsPublished);
            if (string.IsNullOrEmpty(request.StartTime))
            {
                request.StartTime = "1970-01-01";
            }
            if (string.IsNullOrEmpty(request.EndTime))
            {
                request.EndTime = DateTime.MaxValue.ToString();
            }
            exp.AndIF(true, it => it.CreateTime >= request.StartTime.ObjToDate() && it.CreateTime <= request.EndTime.ObjToDate());
            return await base.GetPageList(request, exp.ToExpression());
        }
        /// <summary>
        ///  新增文章
        /// </summary>
        /// <param name="request">文章信息</param>
        /// <returns></returns>
        public async Task<ResultModel<string>> InsertAsync(AddArticleRequest request)
        {
            var response = new ResultModel<string>();
            var article = new ArticleInfo()
            {
                Abstract = request.Abstract,
                Title = request.Title,
                IsPublished = request.IsPublished,
                Likes = request.Likes,
                Views = request.Views,
                Comments = request.Comments,
                ImageUrl = request.ImageUrl
            };
            var content = new ArticleContent()
            {
                Content = request.Content
            };
            var result = await _articleRepository.UseTranAsync(async () =>
            {
                //插入文章基本信息
                var id = await _articleRepository.InsertAsync(article);
                content.ArticleId = id;
                //插入文章内容信息
                await _articleContentRepository.InsertAsync(content);
                var articleTags = request.TagIds.Select(tagId => new ArticleTag()
                {
                    TagId = tagId,
                    ArticleId = id
                }).ToList();
                await _articleTagRepository.InsertAsync(articleTags);
                var articleCategories = request.CategoryIds.Select(categoryId => new ArticleCategory()
                {
                    ArticleId = id,
                    CategoryId = categoryId
                }).ToList();
                await _articleCategoryRepository.InsertAsync(articleCategories);
            });
            response.IsSuccess = result.IsSuccess;
            response.ResultInfo = result.ErrorMessage;
            return response;
        }

        /// <summary>
        ///  更新文章
        /// </summary>
        /// <param name="request">文章信息</param>
        /// <returns></returns>
        public async Task<ResultModel<string>> UpdateAsync(UpdateArticleRequest request)
        {
            var response = new ResultModel<string>();
            var article = new ArticleInfo()
            {
                Abstract = request.Abstract,
                Title = request.Title,
                Id = request.Id,
                Likes = request.Likes,
                Views = request.Views,
                Comments = request.Comments,
                IsPublished = request.IsPublished,
                ImageUrl = request.ImageUrl,
                ModifyTime = DateTime.Now
            };
            var content = new ArticleContent()
            {
                Content = request.Content,
                ArticleId = article.Id,
                ModifyTime = DateTime.Now
            };
            var result = await _articleRepository.UseTranAsync(async () =>
            {
                await _articleRepository.UpdateAsync(article);
                await _articleContentRepository.UpdateAsync(content, it => it.ArticleId);
                ////先删除再添加
                await _articleTagRepository.DeleteAsync(x => x.ArticleId == article.Id);
                await _articleCategoryRepository.DeleteAsync(x => x.ArticleId == article.Id);
                var articleTags = request.TagIds.Select(tagId => new ArticleTag()
                {
                    TagId = tagId,
                    ArticleId = article.Id,
                }).ToList();
                await _articleTagRepository.InsertAsync(articleTags);
                var articleCategories = request.CategoryIds.Select(categoryId => new ArticleCategory()
                {
                    ArticleId = article.Id,
                    CategoryId = categoryId
                }).ToList();
                await _articleCategoryRepository.InsertAsync(articleCategories);
            });

            response.IsSuccess = result.IsSuccess;
            response.ResultInfo = result.ErrorMessage;
            return response;
        }

        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        public async Task<ArticleDetailResponse> GetArticleDetail(int id)
        {
            var response = new ArticleDetailResponse();
            //文章详情
            response.ArticleInfo = await _articleRepository.JoinQuery<ArticleInfo, ArticleContent, ArticleViewModel>(
                   (ai, ac) => new object[] { JoinType.Inner, ai.Id == ac.ArticleId },
                   (ai, ac) => new ArticleViewModel()
                   {
                       Id = ai.Id,
                       Title = ai.Title,
                       Abstract = ai.Abstract,
                       ImageUrl = ai.ImageUrl,
                       Content = ac.Content,
                       Comments = ai.Comments,
                       Likes = ai.Likes,
                       Views = ai.Views,
                       IsPublished = ai.IsPublished,
                       CreateTime = ai.CreateTime
                   },
                   (ai, ac) => ai.Id == id && !ai.IsDeleted
                   );
            //分类信息
            var cIds = await _articleCategoryRepository.Query(x => x.ArticleId == id, x => x.CategoryId);
            response.Categories = await _categoryRepository.Query(cIds, x => new Property()
            {
                Id = x.Id,
                Value = x.CategoryName
            });
            //标签信息
            var tIds = await _articleTagRepository.Query(x => x.ArticleId == id, x => x.TagId);
            response.Tags = await _tagRepository.Query(tIds, x => new Property()
            {
                Id = x.Id,
                Value = x.TagName
            });
            return response;
        }
        /// <summary>
        ///  根据分类获取文章
        /// </summary>
        /// <param name="categoryId">分类id</param>
        /// <param name="param">查询参数</param>
        /// <returns></returns>
        public async Task<JsonResultModel<ArticleInfo>> GetArticleByCategory(int categoryId, GridParams param)
        {
            RefAsync<int> total = 0;
            var articleIds = await _articleCategoryRepository.Query(param,
                x => x.CategoryId == categoryId,
                x => x.ArticleId,
                x => x.ArticleId,
                total
                );
            return new JsonResultModel<ArticleInfo>()
            {
                Rows = await _articleRepository.Query(articleIds),
                TotalRows = total
            };
        }
        /// <summary>
        /// 根据标签获取文章
        /// </summary>
        /// <param name="tagId">标签id</param>
        /// <param name="param">查询参数</param>
        /// <returns></returns>
        public async Task<JsonResultModel<ArticleInfo>> GetArticleByTag(int tagId, GridParams param)
        {
            RefAsync<int> total = 0;
            var articleIds = await _articleTagRepository.Query(param,
                x => x.TagId == tagId,
                x => x.ArticleId,
                x => x.ArticleId,
                total
                );
            return new JsonResultModel<ArticleInfo>()
            {
                Rows = await _articleRepository.Query(articleIds),
                TotalRows = total
            };
        }
    }
}