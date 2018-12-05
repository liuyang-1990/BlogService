using Blog.Model;

namespace Blog.Business
{
    public interface IArticleBusiness:IBaseBusiness<Article>
    {

        bool Insert(ArticleDto articleDto);

        bool Update(ArticleDto articleDto);
    }
}