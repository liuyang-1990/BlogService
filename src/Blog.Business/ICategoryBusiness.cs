using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface ICategoryBusiness : IBaseBusiness<CategoryInfo>
    {

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <param name="categoryName">分类名</param>
        /// <returns></returns>
        Task<JsonResultModel<CategoryInfo>> GetPageList(GridParams param, string categoryName);

        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        Task<List<CategoryInfo>> GetAllCategoryInfos();
    }
}