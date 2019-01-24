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

        [HttpGet("page")]
        public async Task<JsonResultModel<CategoryInfo>> GetPageList(int pageIndex, int pageSize, string categoryName)
        {
            return await _categoryBusiness.GetPageList(pageIndex, pageSize, categoryName);
        }

        [HttpGet("{id}")]
        public async Task<CategoryInfo> GetDetailInfo(int id)
        {
            return await _categoryBusiness.GetDetail(id);
        }

        [HttpPost]
        public async Task<BaseResponse> AddCategory(CategoryRequest category)
        {
            return await _categoryBusiness.Insert(category.CategoryName);
        }

        [HttpDelete("{id}")]
        public async Task<BaseResponse> DeleteCategory(int id)
        {
            return await _categoryBusiness.Delete(id);
        }

        [HttpPut]
        public async Task<BaseResponse> UpdateCategory([FromBody]CategoryInfo category)
        {
            return await _categoryBusiness.Update(category);
        }
    }
}