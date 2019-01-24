using Blog.Model.Db;
using Blog.Model.ViewModel;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface ITagBusiness : IBaseBusiness<TagInfo>
    {
        Task<JsonResultModel<TagInfo>> GetPageList(int pageIndex, int pageSize, string tagName);
    }
}