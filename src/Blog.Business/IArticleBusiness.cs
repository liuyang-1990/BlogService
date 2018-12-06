using Blog.Model.Db;
using Blog.Model.ViewModel;

namespace Blog.Business
{
    public interface IArticleBusiness : IBaseBusiness<ArticleInfo>
    {

        bool Insert(ArticleDto articleDto);

        bool Update(ArticleDto articleDto);
    }
}