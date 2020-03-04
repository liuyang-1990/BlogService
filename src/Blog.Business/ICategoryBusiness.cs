using Blog.Model.Common;
using Blog.Model.Db;
using Blog.Model.Request.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface ICategoryBusiness : IBaseBusiness<CategoryInfo>
    {

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="searchRequest">查询参数</param>
        /// <returns></returns>
        Task<JsonResultModel<CategoryInfo>> GetPageList(CategorySearchRequest searchRequest);

        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        Task<List<CategoryInfo>> GetAllCategoryInfos();
    }
}