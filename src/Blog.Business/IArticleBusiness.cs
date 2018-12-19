using System.Threading.Tasks;
using Blog.Model.Db;
using Blog.Model.Response;
using Blog.Model.ViewModel;

namespace Blog.Business
{
    public interface IArticleBusiness : IBaseBusiness<ArticleInfo>
    {

        Task<BaseResponse> Insert(ArticleDto articleDto);

        bool Update(ArticleDto articleDto);
    }
}