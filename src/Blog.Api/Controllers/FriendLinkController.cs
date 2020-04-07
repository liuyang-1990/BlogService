using Blog.Business;
using Blog.Model.Db;
using Blog.Model.Request.FrendLink;
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
    public class FriendLinkController : ControllerBase
    {
        private readonly IFriendLinkBusiness _friendLinkBusiness;

        public FriendLinkController(IFriendLinkBusiness friendLinkBusiness)
        {
            _friendLinkBusiness = friendLinkBusiness;
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IEnumerable<FriendLink>> GetAll()
        {
            return await _friendLinkBusiness.QueryAllAsync();
        }

        /// <summary>
        /// 新增友链
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody]CommonFriendLinkRequest request)
        {
            var success = await _friendLinkBusiness.InsertAsync(TinyMapper.Map<FriendLink>(request));
            if (success)
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// 更新友链
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody]UpdateFriendLinkRequest request)
        {
            var success = await _friendLinkBusiness.UpdateAsync(TinyMapper.Map<FriendLink>(request));
            if (success)
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// 删除友链
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            int.TryParse(id, out var cid);
            var success = await _friendLinkBusiness.SoftDeleteAsync(cid);
            if (success)
            {
                return Ok();
            }
            return BadRequest();
        }

    }
}