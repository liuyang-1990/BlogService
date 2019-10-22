﻿using Blog.Business;
using Blog.Model.Db;
using Blog.Model.Request;
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

        public CategoryController(ICategoryBusiness categoryBusiness)
        {
            _categoryBusiness = categoryBusiness;
        }
        /// <summary>
        /// 分页获取分类信息
        /// </summary>
        /// <param name="param"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        [HttpGet("page")]
        public async Task<JsonResultModel<CategoryInfo>> GetPageList([FromQuery]GridParams param, string categoryName)
        {
            return await _categoryBusiness.GetPageList(param, categoryName);
        }
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
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultModel<string>> AddCategory([FromBody]CategoryInfo category)
        {
            return await _categoryBusiness.Insert(category);
        }
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ResultModel<string>> DeleteCategory(int id)
        {
            return await _categoryBusiness.Delete(id);
        }
        /// <summary>
        /// 更新分类
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResultModel<string>> UpdateCategory([FromBody]CategoryInfo category)
        {
            return await _categoryBusiness.Update(category);
        }
    }
}