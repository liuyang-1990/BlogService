using Blog.Model.Db;

namespace Blog.Repository
{
    public interface IArticleRepository : IBaseRepository<ArticleInfo>
    {
        bool Insert(ArticleInfo article, ArticleContent content);

        bool Update(ArticleInfo article, ArticleContent content);
    }
}