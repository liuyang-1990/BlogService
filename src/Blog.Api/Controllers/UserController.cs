using Blog.Business;
using Blog.Model.Common;
using Blog.Model.Db;
using Blog.Model.Request.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nelibur.ObjectMapper;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [ApiController]
    [Route("user")]
    [EnableCors("LimitRequests")]//支持跨域
    [Authorize(Policy = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;
        public UserController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        /// <summary>
        /// 分页获取用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("page")]
        public async Task<JsonResultModel<UserInfo>> GetPageList([FromQuery]UserSearchRequest request)
        {
            return await _userBusiness.GetPageList(request);
        }

        /// <summary>
        /// 获取某个用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<UserInfo> GetDetailInfo(string id)
        {
            int.TryParse(id, out var uid);
            return await _userBusiness.SingleAsync(uid);
        }

        /// <summary>
        /// 新增用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody]AddUserRequest request)
        {
            var success = await _userBusiness.InsertAsync(TinyMapper.Map<UserInfo>(request));
            if (success)
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody]UpdateUserRequest request)
        {
            var success = await _userBusiness.UpdateAsync(TinyMapper.Map<UserInfo>(request));
            if (success)
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            int.TryParse(id, out var uid);
            var success = await _userBusiness.SoftDeleteAsync(uid);
            if (success)
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("password")]
        public async Task<IActionResult> UpdatePassword([FromBody]ChangePasswordRequest request)
        {
            var success = await _userBusiness.UpdatePassword(request);
            if (success)
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("status")]
        public async Task<IActionResult> UpdateStatus([FromBody]UpdateStatusRequest request)
        {
            var success = await _userBusiness.UpdateStatus(request);
            if (success)
            {
                return Ok();
            }
            return BadRequest();
        }

    }

    //public class CriteriaContractResolver : DefaultContractResolver
    //{
    //    readonly IEnumerable<string> _ignoreProperties;

    //    public CriteriaContractResolver(IEnumerable<string> ignoreProperties)
    //    {
    //        _ignoreProperties = ignoreProperties;
    //    }

    //    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    //    {
    //        JsonProperty property = base.CreateProperty(member, memberSerialization);
    //        property.ShouldSerialize = instance => !_ignoreProperties.Contains(property.PropertyName);
    //        return property;
    //    }
    //}
}