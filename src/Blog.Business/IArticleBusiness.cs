using Blog.Model.Db;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface IArticleBusiness : IBaseBusiness<ArticleInfo>
    {

        Task<ResultModel<string>> Insert(ArticleDto articleDto);

        Task<ResultModel<string>> Update(ArticleDto articleDto);

        Task<ArticleDetailResponse> GetArticleDetail(int id);

        Task<List<ArticleInfo>> GetArticleByCategory(int categoryId, int pageIndex, int pageSize);

        Task<List<ArticleInfo>> GetArticleByTag(int tagId, int pageIndex, int pageSize);
    }
}