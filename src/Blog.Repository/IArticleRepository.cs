using Blog.Model;

namespace Blog.Repository
{
    public interface IArticleRepository
    {
        bool Insert(Article article, ArticleContent content);

        bool Delete(int id);

        bool Update(Article article, ArticleContent content);
    }
}