using Blog.Business;
using Blog.Model.Common;
using Blog.Model.Db;
using Blog.Model.Request.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nelibur.ObjectMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
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
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        [HttpGet("page")]
        public async Task<JsonResultModel<CategoryInfo>> GetPageList([FromQuery]CategorySearchRequest searchRequest)
        {
            return await _categoryBusiness.GetPageList(searchRequest);
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IEnumerable<CategoryInfo>> GetAll()
        {
            return await _categoryBusiness.GetAllCategoryInfos();
        }

        /// <summary>
        /// 获取某个分类的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<CategoryInfo> GetDetailInfo(int id)
        {
            return await _categoryBusiness.SingleAsync(id);
        }
        /// <summary>
        /// 新增分类
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody]CommonCategoryRequest request)
        {
            var success = await _categoryBusiness.InsertAsync(TinyMapper.Map<CategoryInfo>(request));
            if (success)
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// 更新分类
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody]UpdateCategoryRequest request)
        {
            var success = await _categoryBusiness.UpdateAsync(TinyMapper.Map<CategoryInfo>(request));
            if (success)
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var success = await _categoryBusiness.SoftDeleteAsync(id);
            if (success)
            {
                return Ok();
            }
            return BadRequest();
        }

    }
}