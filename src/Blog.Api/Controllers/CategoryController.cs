using Blog.Business;
using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [BlogApiController]
    [EnableCors("LimitRequests")]//支持跨域
    [Authorize(Policy = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryBusiness _categoryBusiness;

        public CategoryController(ICategoryBusiness categoryBusiness)
        {
            _categoryBusiness = categoryBusiness;
        }
        /// <summary>
        /// 分页获取分类信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        [HttpGet("page")]
        public async Task<JsonResultModel<CategoryInfo>> GetPageList(int pageIndex, int pageSize, string categoryName)
        {
            return await _categoryBusiness.GetPageList(pageIndex, pageSize, categoryName);
        }
        /// <summary>
        /// 获取某个分类的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<CategoryInfo> GetDetailInfo(int id)
        {
            return await _categoryBusiness.GetDetail(id);
        }
        /// <summary>
        /// 新增分类
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> AddCategory(CategoryRequest category)
        {
            return await _categoryBusiness.Insert(category.CategoryName);
        }
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<BaseResponse> DeleteCategory(int id)
        {
            return await _categoryBusiness.Delete(id);
        }
        /// <summary>
        /// 更新分类
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<BaseResponse> UpdateCategory([FromBody]CategoryInfo category)
        {
            return await _categoryBusiness.Update(category);
        }
    }
}