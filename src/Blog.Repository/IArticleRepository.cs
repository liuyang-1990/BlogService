using Blog.Model.Db;
using Blog.Model.Response;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Model.Request;
using Blog.Model.ViewModel;

namespace Blog.Repository
{
    public interface IArticleRepository : IBaseRepository<ArticleInfo>
    {
        Task<bool> Insert(ArticleInfo article, ArticleContent content, List<string> tags, List<string> categoryIds);

        Task<bool> Update(ArticleInfo article, ArticleContent content, List<string> tags, List<string> categoryIds);

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