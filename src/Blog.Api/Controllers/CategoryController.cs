using Blog.Business;
using Blog.Model.Db;
using Microsoft.AspNetCore.Mvc;

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
        public string GetPageList(int pageIndex, int pageSize)
        {
            return _categoryBusiness.GetPageList(pageIndex, pageSize);
        }

        [HttpGet("{id}")]
        public string GetDetailInfo(int id)
        {
            return _categoryBusiness.GetDetail(id);
        }

        [HttpPost]
        public bool AddCategory([FromBody]Category category)
        {

            return _categoryBusiness.Insert(category);
        }

        [HttpDelete("{id}")]
        public bool DeleteCategory(int id)
        {
            return _categoryBusiness.Delete(id);
        }

        [HttpPut]
        public bool UpdateCategory([FromBody]Category category)
        {
            return _categoryBusiness.Update(category);
        }
    }
}