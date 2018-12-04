using Blog.Model;

namespace Blog.Repository
{
    public interface IArticleRepository
    {
        string GetPageList(int pageIndex, int pageSize);

        string GetDetailInfo(int id);

        bool Insert(Article article, ArticleContent content);

        bool Delete(int id);

        bool Update(Article article, ArticleContent content);

        
    }
}