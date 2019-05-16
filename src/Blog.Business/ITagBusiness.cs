using System.Collections.Generic;
using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.ViewModel;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface ITagBusiness : IBaseBusiness<TagInfo>
    {
        Task<JsonResultModel<TagInfo>> GetPageList(GridParams param, string tagName);

        Task<IEnumerable<TagInfo>> GetAllTags();
    }
}