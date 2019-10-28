using AspectCore.Injector;
using Blog.Model;
using Blog.Model.Db;
using Blog.Model.Request;
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
        /// <param name="param"></param>
        /// <param name="searchParmas"></param>
        /// <returns></returns>
        public async Task<JsonResultModel<ArticleInfo>> GetPageList(GridParams param, ArticleRequest searchParmas)
        {
            var exp = Expressionable.Create<ArticleInfo>().AndIF(searchParmas.Status.HasValue, it => it.Status == searchParmas.Status);
            if (string.IsNullOrEmpty(searchParmas.StartTime))
            {
                searchParmas.StartTime = "1970-01-01";
            }
            if (string.IsNullOrEmpty(searchParmas.EndTime))
            {
                searchParmas.EndTime = DateTime.MaxValue.ToString();
            }
            exp.AndIF(true, it => it.CreateTime >= searchParmas.StartTime.ObjToDate() && it.CreateTime <= searchParmas.EndTime.ObjToDate());
            return await base.GetPageList(param, exp.ToExpression());
        }
        /// <summary>
        ///  新增文章
        /// </summary>
        /// <param name="articleDto">文章信息</param>
        /// <returns></returns>
        public async Task<ResultModel<string>> Insert(ArticleDto articleDto)
        {
            var response = new ResultModel<string>();
            var article = new ArticleInfo()
            {
                Abstract = articleDto.Abstract,
                Title = articleDto.Title,
                IsOriginal = articleDto.IsOriginal,
                Status = articleDto.Status,
                ImageUrl = articleDto.ImageUrl
            };
            var content = new ArticleContent()
            {
                Content = articleDto.Content
            };
            response.IsSuccess = await _articleRepository.Insert(article, content, articleDto.TagIds, articleDto.CategoryIds);
            response.Status = response.IsSuccess ? "0" : "1";

            return response;
        }

        /// <summary>
        ///  更新文章
        /// </summary>
        /// <param name="articleDto">文章信息</param>
        /// <returns></returns>
        public async Task<ResultModel<string>> Update(ArticleDto articleDto)
        {
            var response = new ResultModel<string>();
            var article = new ArticleInfo()
            {
                Abstract = articleDto.Abstract,
                Title = articleDto.Title,
                Id = articleDto.Id,
                Likes = articleDto.Likes,
                Views = articleDto.Views,
                Comments = articleDto.Comments,
                IsOriginal = articleDto.IsOriginal,
                Status = articleDto.Status,
                ImageUrl = articleDto.ImageUrl,
                ModifyTime = DateTime.Now
            };
            var content = new ArticleContent()
            {
                Content = articleDto.Content,
                ArticleId = article.Id,
                ModifyTime = DateTime.Now
            };
            response.IsSuccess = await _articleRepository.Update(article, content, articleDto.TagIds, articleDto.CategoryIds);
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