using Blog.Model.Db;
using Blog.Model.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Repository
{
    public interface IArticleRepository : IBaseRepository<ArticleInfo>
    {
        Task<bool> Insert(ArticleInfo article, ArticleContent content, string[] tagIds, string[] categoryIds);

        Task<bool> Update(ArticleInfo article, ArticleContent content, string[] tagIds, string[] categoryIds);

        Task<V_Article_Info> GetArticleDetail(int id);

        Task<List<ArticleInfo>> GetArticleByCategory(int categoryId, int pageIndex, int pageSize);

        Task<List<ArticleInfo>> GetArticleByTag(int tagId, int pageIndex, int pageSize);
    }
}