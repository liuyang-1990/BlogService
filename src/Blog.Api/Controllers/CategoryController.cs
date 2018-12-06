using Blog.Business;
using Blog.Model.Db;
using Blog.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [BlogApiController]
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
        public async Task<bool> AddCategory([FromBody]CategoryInfo category)
        {

            return await _categoryBusiness.Insert(category);
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteCategory(int id)
        {
            return await _categoryBusiness.Delete(id);
        }

        [HttpPut]
        public async Task<bool> UpdateCategory([FromBody]CategoryInfo category)
        {
            return await _categoryBusiness.Update(category);
        }
    }
}