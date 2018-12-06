using Blog.Model.Db;
using Blog.Model.ViewModel;

namespace Blog.Business
{
    public interface IArticleBusiness : IBaseBusiness<Article>
    {

        bool Insert(ArticleDto articleDto);

        bool Update(ArticleDto articleDto);
    }
}