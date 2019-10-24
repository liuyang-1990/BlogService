using Blog.Model.Db;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Model.Request;

namespace Blog.Business
{
    public interface IArticleBusiness : IBaseBusiness<ArticleInfo>
    {
        Task<JsonResultModel<ArticleInfo>> GetPageList(GridParams param, ArticleRequest searchParmas);
        Task<ResultModel<string>> Insert(ArticleDto articleDto);

        Task<ResultModel<string>> Update(ArticleDto articleDto);

        Task<ArticleDetailResponse> GetArticleDetail(string id);

        Task<List<ArticleInfo>> GetArticleByCategory(string categoryId, int pageIndex, int pageSize);

        Task<List<ArticleInfo>> GetArticleByTag(string tagId, int pageIndex, int pageSize);
    }
}