using Blog.Model.Db;
using Blog.Model.Request;
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
        /// <param name="param"></param>
        /// <param name="searchParmas"></param>
        /// <returns></returns>
        Task<JsonResultModel<ArticleInfo>> GetPageList(GridParams param, ArticleRequest searchParmas);
        /// <summary>
        ///  新增文章
        /// </summary>
        /// <param name="articleDto">文章信息</param>
        /// <returns></returns>
        Task<ResultModel<string>> Insert(ArticleDto articleDto);
        /// <summary>
        ///  更新文章
        /// </summary>
        /// <param name="articleDto">文章信息</param>
        /// <returns></returns>
        Task<ResultModel<string>> Update(ArticleDto articleDto);
        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ArticleDetailResponse> GetArticleDetail(string id);
        /// <summary>
        ///  根据分类获取文章
        /// </summary>
        /// <param name="categoryId">分类id</param>
        /// <param name="param">查询参数</param>
        /// <returns></returns>
        Task<JsonResultModel<ArticleInfo>> GetArticleByCategory(string categoryId, GridParams param);
        /// <summary>
        /// 根据标签获取文章
        /// </summary>
        /// <param name="tagId">标签id</param>
        /// <param name="param">查询参数</param>
        /// <returns></returns>
        Task<JsonResultModel<ArticleInfo>> GetArticleByTag(string tagId, GridParams param);
    }
}