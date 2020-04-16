using Blog.Business;
using Blog.Model.Common;
using Blog.Model.Db;
using Blog.Model.Request.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nelibur.ObjectMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [ApiController]
    [Route("tag")]
    [EnableCors("LimitRequests")]//支持跨域
    [Authorize(Policy = "Admin")]
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
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        [HttpGet("page")]
        public async Task<JsonResultModel<TagInfo>> GetPageList([FromQuery]TagSearchRequest searchRequest)
        {
            return await _tagBusiness.GetPageList(searchRequest);
        }

        [HttpGet("all")]
        [AllowAnonymous]
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
        public async Task<TagInfo> GetDetailInfo(string id)
        {
            int.TryParse(id, out var tid);
            return await _tagBusiness.SingleAsync(tid);
        }
        /// <summary>
        /// 新增标签
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddTag([FromBody]CommonTagRequest request)
        {
            var success = await _tagBusiness.InsertAsync(TinyMapper.Map<TagInfo>(request));
            if (success)
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateTag([FromBody]UpdateTagRequest request)
        {
            var success = await _tagBusiness.UpdateAsync(TinyMapper.Map<TagInfo>(request));
            if (success)
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(string id)
        {
            int.TryParse(id, out var tid);
            var success = await _tagBusiness.SoftDeleteAsync(tid);
            if (success)
            {
                return Ok();
            }
            return BadRequest();
        }

    }
}