using Blog.Model.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Repository
{
    public interface IArticleRepository : IBaseRepository<ArticleInfo>
    {
        Task<bool> Insert(ArticleInfo article, ArticleContent content, List<string> tags, List<string> categoryIds);

        Task<bool> Update(ArticleInfo article, ArticleContent content, List<string> tags, List<string> categoryIds);

    }
}