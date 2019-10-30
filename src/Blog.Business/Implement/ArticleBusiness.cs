using AspectCore.Injector;
using Blog.Model;
using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.Request.Article;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Blog.Repository;
using SqlSugar;
using System;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{
    [Injector(typeof(IArticleBusiness), LifeTime = Lifetime.Scoped)]
    public class ArticleBusiness : BaseBusiness<ArticleInfo>, IArticleBusiness
    {

        private readonly IArticleRepository _articleRepository;

        public ArticleBusiness(IArticleRepository articleRepository)
        {
            BaseRepository = articleRepository;
            _articleRepository = articleRepository;
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<JsonResultModel<ArticleInfo>> GetPageList(ArticleSearchRequest request)
        {
            var exp = Expressionable.Create<ArticleInfo>().AndIF(request.Status.HasValue, it => it.Status == request.Status);
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
        public async Task<ResultModel<string>> Insert(AddArticleRequest request)
        {
            var response = new ResultModel<string>();
            var article = new ArticleInfo()
            {
                Abstract = request.Abstract,
                Title = request.Title,
                IsOriginal = request.IsOriginal,
                Status = request.Status,
                Likes = request.Likes,
                Views = request.Views,
                Comments = request.Comments,
                ImageUrl = request.ImageUrl
            };
            var content = new ArticleContent()
            {
                Content = request.Content
            };
            response.IsSuccess = await _articleRepository.Insert(article, content, request.TagIds, request.CategoryIds);
            response.Status = response.IsSuccess ? "0" : "1";

            return response;
        }

        /// <summary>
        ///  更新文章
        /// </summary>
        /// <param name="request">文章信息</param>
        /// <returns></returns>
        public async Task<ResultModel<string>> Update(UpdateArticleRequest request)
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
                IsOriginal = request.IsOriginal,
                Status = request.Status,
                ImageUrl = request.ImageUrl,
                ModifyTime = DateTime.Now
            };
            var content = new ArticleContent()
            {
                Content = request.Content,
                ArticleId = article.Id,
                ModifyTime = DateTime.Now
            };
            response.IsSuccess = await _articleRepository.Update(article, content, request.TagIds, request.CategoryIds);
            response.Status = response.IsSuccess ? "0" : "1";
            return response;
        }

        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        public async Task<ArticleDetailResponse> GetArticleDetail(string id)
        {
            return await _articleRepository.GetArticleDetail(id);
        }
        /// <summary>
        ///  根据分类获取文章
        /// </summary>
        /// <param name="categoryId">分类id</param>
        /// <param name="param">查询参数</param>
        /// <returns></returns>
        public async Task<JsonResultModel<ArticleInfo>> GetArticleByCategory(string categoryId, GridParams param)
        {
            return await _articleRepository.GetArticleByCategory(categoryId, param);
        }
        /// <summary>
        /// 根据标签获取文章
        /// </summary>
        /// <param name="tagId">标签id</param>
        /// <param name="param">查询参数</param>
        /// <returns></returns>
        public async Task<JsonResultModel<ArticleInfo>> GetArticleByTag(string tagId, GridParams param)
        {
            return await _articleRepository.GetArticleByTag(tagId, param);
        }
    }
}