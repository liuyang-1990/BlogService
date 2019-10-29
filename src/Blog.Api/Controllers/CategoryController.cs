using AutoMapper;
using Blog.Business;
using Blog.Model.Db;
using Blog.Model.Request.Category;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;
        public CategoryController(ICategoryBusiness categoryBusiness, IMapper mapper)
        {
            _categoryBusiness = categoryBusiness;
            _mapper = mapper;
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
        public async Task<CategoryInfo> GetDetailInfo(string id)
        {
            return await _categoryBusiness.GetDetail(id);
        }
        /// <summary>
        /// 新增分类
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultModel<string>> AddCategory([FromBody]CommonCategoryRequest request)
        {
            return await _categoryBusiness.Insert(_mapper.Map<CategoryInfo>(request));
        }

        /// <summary>
        /// 更新分类
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResultModel<string>> UpdateCategory([FromBody]UpdateCategoryRequest request)
        {
            return await _categoryBusiness.Update(_mapper.Map<CategoryInfo>(request));
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ResultModel<string>> DeleteCategory(string id)
        {
            return await _categoryBusiness.Delete(id);
        }

    }
}