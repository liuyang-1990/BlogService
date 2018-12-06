using Blog.Model.Db;

namespace Blog.Repository
{
    public interface IArticleRepository : IBaseRepository<Article>
    {
        bool Insert(Article article, ArticleContent content);

        bool Update(Article article, ArticleContent content);
    }
}