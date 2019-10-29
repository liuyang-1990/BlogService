using System.Collections.Generic;
using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.ViewModel;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface ITagBusiness : IBaseBusiness<TagInfo>
    { 
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <param name="tagName">标签名</param>
        /// <returns></returns>
        Task<JsonResultModel<TagInfo>> GetPageList(GridParams param, string tagName);

        /// <summary>
        ///  获取所有标签
        /// </summary>
        /// <returns></returns>
        Task<List<TagInfo>> GetAllTags();
    }
}