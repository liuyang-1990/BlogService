using Blog.Model.Db;
using Blog.Model.Request.Tag;
using Blog.Model.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface ITagBusiness : IBaseBusiness<TagInfo>
    {
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="searchRequest">查询参数</param>
        /// <returns></returns>
        Task<JsonResultModel<TagInfo>> GetPageList(TagSearchRequest searchRequest);

        /// <summary>
        ///  获取所有标签
        /// </summary>
        /// <returns></returns>
        Task<List<TagInfo>> GetAllTags();
    }
}