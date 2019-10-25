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