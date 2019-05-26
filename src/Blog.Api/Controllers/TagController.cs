using Blog.Business;
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
    [EnableCors("LimitRequests")]//支持跨域
    [BlogApiController]
    //[Authorize(Policy = "Admin")]
    public class TagController : ControllerBase
    {
        private readonly ITagBusiness _tagBusiness;
        public TagController(ITagBusiness tagBusiness)
        {
            _tagBusiness = tagBusiness;
        }

        /// <summary>
        /// 分页获取标签信息
        /// </summary>
        /// <param name="param"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        [HttpGet("page")]
        public async Task<JsonResultModel<TagInfo>> GetPageList(GridParams param, string tagName)
        {
            return await _tagBusiness.GetPageList(param, tagName);
        }

        [HttpGet("all")]
        public async Task<IEnumerable<TagInfo>> GetAll()
        {
            return await _tagBusiness.GetAllTags();
        }
        /// <summary>
        /// 获取某个标签的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<TagInfo> GetDetailInfo(int id)
        {
            return await _tagBusiness.GetDetail(id);
        }
        /// <summary>
        /// 新增标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultModel<string>> AddTag([FromBody]TagInfo tag)
        {
            return await _tagBusiness.Insert(tag);
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ResultModel<string>> DeleteTag(int id)
        {
            return await _tagBusiness.Delete(id);
        }
        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResultModel<string>> UpdateTag([FromBody]TagInfo tag)
        {
            return await _tagBusiness.Update(tag);
        }
    }
}