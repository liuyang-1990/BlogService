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
            var exp = Expressionable.Create<ArticleInfo>().AndIF(true, it => it.Status == searchParmas.Status);
            if (!string.IsNullOrEmpty(searchParmas.StartTime) && string.IsNullOrEmpty(searchParmas.EndTime))
            {
                exp.AndIF(true, it => it.CreateTime >= searchParmas.StartTime.ObjToDate());
            }
            else if (!string.IsNullOrEmpty(searchParmas.EndTime) && DateTime.TryParse(searchParmas.EndTime, out _) && string.IsNullOrEmpty(searchParmas.StartTime))
            {
                exp.AndIF(true, it => it.CreateTime <= searchParmas.EndTime.ObjToDate());
            }
            else if (!string.IsNullOrEmpty(searchParmas.StartTime) && !string.IsNullOrEmpty(searchParmas.EndTime))
            {
                var res = DateTime.Compare(searchParmas.StartTime.ObjToDate(), searchParmas.EndTime.ObjToDate());
                if (res < 0)
                {
                    exp.AndIF(true, it => it.CreateTime > searchParmas.StartTime.ObjToDate() && it.CreateTime < searchParmas.EndTime.ObjToDate());
                }
                else if (res == 0)
                {
                    exp.AndIF(true, it => it.CreateTime > searchParmas.StartTime.ObjToDate() && it.CreateTime < searchParmas.StartTime.ObjToDate().AddDays(1));
                }
                else if (res > 0)
                {
                    exp.AndIF(true, it => it.CreateTime > searchParmas.EndTime.ObjToDate() && it.CreateTime < searchParmas.StartTime.ObjToDate());
                }
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