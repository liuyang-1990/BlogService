using Blog.Model.Db;
using Blog.Model.ViewModel;
using System.Threading.Tasks;

namespace Blog.Repository
{
    public interface IArticleRepository : IBaseRepository<ArticleInfo>
    {
        Task<bool> Insert(ArticleInfo article, ArticleContent content, string[] tagIds, string[] categoryIds);

        Task<bool> Update(ArticleInfo article, ArticleContent content, string[] tagIds, string[] categoryIds);

        Task<V_Article_Info> GetArticleDetail(int id);
    }
}