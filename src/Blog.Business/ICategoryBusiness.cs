using Blog.Model.Db;
using Blog.Model.ViewModel;
using System.Threading.Tasks;
using Blog.Model.Response;

namespace Blog.Business
{
    public interface ICategoryBusiness : IBaseBusiness<CategoryInfo>
    {
        Task<JsonResultModel<CategoryInfo>> GetPageList(int pageIndex, int pageSize, string categoryName);

        Task<BaseResponse> Insert(string categoryName);
    }
}