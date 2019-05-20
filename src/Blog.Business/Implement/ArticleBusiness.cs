using Blog.Model.Db;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Blog.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{
    public class ArticleBusiness : BaseBusiness<ArticleInfo>, IArticleBusiness
    {

        private readonly IArticleRepository _articleRepository;

        public ArticleBusiness(IArticleRepository articleRepository)
        {
            BaseRepository = articleRepository;
            _articleRepository = articleRepository;
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
            var tagIds = articleDto.Tags.Split(",", StringSplitOptions.RemoveEmptyEntries);
            // var categoryIds = articleDto.Categories.Split(",", StringSplitOptions.RemoveEmptyEntries);
            response.IsSuccess = await _articleRepository.Insert(article, content, tagIds, articleDto.Category);
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
                ModifyTime = DateTime.Now
            };
            var content = new ArticleContent()
            {
                Content = articleDto.Content,
                ArticleId = article.Id,
                ModifyTime = DateTime.Now
            };
            var tagIds = articleDto.Tags.Split(",", StringSplitOptions.RemoveEmptyEntries);
            //var categoryIds = articleDto.Categories.Split(",", StringSplitOptions.RemoveEmptyEntries);
            response.IsSuccess = await _articleRepository.Update(article, content, tagIds, articleDto.Category);
            response.Status = response.IsSuccess ? "0" : "1";
            return response;
        }

        public async Task<ArticleDetailResponse> GetArticleDetail(int id)
        {
            return await _articleRepository.GetArticleDetail(id);
        }

        public async Task<List<ArticleInfo>> GetArticleByCategory(int categoryId, int pageIndex, int pageSize)
        {
            return await _articleRepository.GetArticleByCategory(categoryId, pageIndex, pageSize);
        }

        public async Task<List<ArticleInfo>> GetArticleByTag(int tagId, int pageIndex, int pageSize)
        {
            return await _articleRepository.GetArticleByTag(tagId, pageIndex, pageSize);
        }
    }
}