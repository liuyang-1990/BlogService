using AspectCore.Injector;
using Blog.Model;
using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Blog.Repository;
using SqlSugar;
using System;
using System.Collections.Generic;
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

        public async Task<JsonResultModel<ArticleInfo>> GetPageList(GridParams param, ArticleRequest searchParmas)
        {
            var exp = Expressionable.Create<ArticleInfo>().AndIF(searchParmas.Status.HasValue, it => it.Status == searchParmas.Status);
            if (!string.IsNullOrEmpty(searchParmas.StartTime) && string.IsNullOrEmpty(searchParmas.EndTime))
            {
                exp.AndIF(true, it => it.CreateTime >= searchParmas.StartTime.ObjToDate());
            }
            else if (!string.IsNullOrEmpty(searchParmas.EndTime) && string.IsNullOrEmpty(searchParmas.StartTime))
            {
                exp.AndIF(true, it => it.CreateTime <= searchParmas.EndTime.ObjToDate());
            }
            else if (!string.IsNullOrEmpty(searchParmas.StartTime) && !string.IsNullOrEmpty(searchParmas.EndTime))
            {
                exp.AndIF(true, it => it.CreateTime >= searchParmas.StartTime.ObjToDate() && it.CreateTime <= searchParmas.EndTime.ObjToDate());
            }
            return await base.GetPageList(param, exp.ToExpression());
        }

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
            response.IsSuccess = await _articleRepository.Insert(article, content, articleDto.Tags, articleDto.Categories);
            response.Status = response.IsSuccess ? "0" : "1";

            return response;
        }

        public async Task<ResultModel<string>> Update(ArticleDto articleDto)
        {
            var response = new ResultModel<string>();
            var article = new ArticleInfo()
            {
                Abstract = articleDto.Abstract,
                Title = articleDto.Title,
                Id = articleDto.Id,
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
            response.IsSuccess = await _articleRepository.Update(article, content, articleDto.Tags, articleDto.Categories);
            response.Status = response.IsSuccess ? "0" : "1";
            return response;
        }

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