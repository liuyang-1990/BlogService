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


        public async Task<BaseResponse> Insert(ArticleDto articleDto)
        {
            var response = new BaseResponse();
            var article = new ArticleInfo()
            {
                Abstract = articleDto.Abstract,
                Title = articleDto.Title
            };
            var content = new ArticleContent()
            {
                Content = articleDto.Content
            };
            var tagIds = articleDto.Tags.Split(",", StringSplitOptions.RemoveEmptyEntries);
            var categoryIds = articleDto.Categories.Split(",", StringSplitOptions.RemoveEmptyEntries);
            try
            {
                var isSuccess = await _articleRepository.Insert(article, content, tagIds, categoryIds);
                if (isSuccess)
                {
                    response.Code = (int)ResponseStatus.Ok;
                    response.Msg = MessageConst.Created;
                }
                else
                {
                    response.Code = (int)ResponseStatus.Fail;
                    response.Msg = MessageConst.Fail;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)ResponseStatus.Fail;
                response.Msg = ex.Message;
            }
            return response;
        }

        public async Task<BaseResponse> Update(ArticleDto articleDto)
        {
            var response = new BaseResponse();
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
            var categoryIds = articleDto.Categories.Split(",", StringSplitOptions.RemoveEmptyEntries);
            try
            {
                var isSuccess = await _articleRepository.Update(article, content, tagIds, categoryIds);
                if (isSuccess)
                {
                    response.Code = (int)ResponseStatus.Ok;
                    response.Msg = MessageConst.Created;
                }
                else
                {
                    response.Code = (int)ResponseStatus.Fail;
                    response.Msg = MessageConst.Fail;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)ResponseStatus.Fail;
                response.Msg = ex.Message;
            }
            return response;
        }

        public async Task<V_Article_Info> GetArticleDetail(int id)
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