using Blog.Model.Db;
using System.Threading.Tasks;

namespace Blog.Repository
{
    public interface IArticleRepository : IBaseRepository<ArticleInfo>
    {
        Task<bool> Insert(ArticleInfo article, ArticleContent content, string tagIds, string categoryIds);

        bool Update(ArticleInfo article, ArticleContent content);
    }
}