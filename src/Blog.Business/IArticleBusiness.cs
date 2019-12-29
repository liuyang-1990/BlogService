using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.Request.Article;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface IArticleBusiness : IBaseBusiness<ArticleInfo>
    {
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<JsonResultModel<ArticleInfo>> GetPageList(ArticleSearchRequest request);
        /// <summary>
        ///  新增文章
        /// </summary>
        /// <param name="request">文章信息</param>
        /// <returns></returns>
        Task<ResultModel<string>> InsertAsync(AddArticleRequest request);
        /// <summary>
        ///  更新文章
        /// </summary>
        /// <param name="request">文章信息</param>
        /// <returns></returns>
        Task<ResultModel<string>> UpdateAsync(UpdateArticleRequest request);
        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ArticleDetailResponse> GetArticleDetail(int id);
        /// <summary>
        ///  根据分类获取文章
        /// </summary>
        /// <param name="categoryId">分类id</param>
        /// <param name="param">查询参数</param>
        /// <returns></returns>
        Task<JsonResultModel<ArticleInfo>> GetArticleByCategory(int categoryId, GridParams param);
        /// <summary>
        /// 根据标签获取文章
        /// </summary>
        /// <param name="tagId">标签id</param>
        /// <param name="param">查询参数</param>
        /// <returns></returns>
        Task<JsonResultModel<ArticleInfo>> GetArticleByTag(int tagId, GridParams param);
    }
}