using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface ICategoryBusiness : IBaseBusiness<CategoryInfo>
    {
        Task<JsonResultModel<CategoryInfo>> GetPageList(GridParams param, string categoryName);

        Task<ResultModel<string>> Insert(string categoryName);
    }
}