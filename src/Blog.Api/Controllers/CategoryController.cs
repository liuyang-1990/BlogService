using Blog.Business;
using Blog.Model.Db;
using Blog.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Blog.Model.Response;
using Microsoft.AspNetCore.Cors;

namespace Blog.Api.Controllers
{
    [BlogApiController]
    [EnableCors("LimitRequests")]//支持跨域
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryBusiness _categoryBusiness;

        public CategoryController(ICategoryBusiness categoryBusiness)
        {
            _categoryBusiness = categoryBusiness;
        }

        [HttpGet("page")]
        public async Task<JsonResultModel<CategoryInfo>> GetPageList(int pageIndex, int pageSize)
        {
            return await _categoryBusiness.GetPageList(pageIndex, pageSize, null);
        }

        [HttpGet("{id}")]
        public async Task<CategoryInfo> GetDetailInfo(int id)
        {
            return await _categoryBusiness.GetDetail(id);
        }

        [HttpPost]
        public async Task<BaseResponse> AddCategory([FromBody]CategoryInfo category)
        {
            return await _categoryBusiness.Insert(category);
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