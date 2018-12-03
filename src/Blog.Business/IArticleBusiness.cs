using Blog.Model;

namespace Blog.Business
{
    public interface IArticleBusiness
    {
        bool Insert(ArticleDto articleDto);

        bool Delete(int id);

        bool Update(ArticleDto articleDto);
    }
}