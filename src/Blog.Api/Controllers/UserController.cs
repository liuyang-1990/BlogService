using AutoMapper;
using Blog.Business;
using Blog.Model.Db;
using Blog.Model.Request.User;
using Blog.Model.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [BlogApiController]
    [EnableCors("LimitRequests")]//支持跨域
    [Authorize(Policy = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IMapper _mapper;
        public UserController(IUserBusiness userBusiness, IMapper mapper)
        {
            _userBusiness = userBusiness;
            _mapper = mapper;
        }

        /// <summary>
        /// 分页获取用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("page")]
        public async Task<JsonResult> GetPageList([FromQuery]UserSearchRequest request)
        {
            var settings = new JsonSerializerSettings()
            {
                //ignore these properties because not used 
                ContractResolver = new CriteriaContractResolver(new List<string>()
                {
                    "Password"
                })
            };
            var model = await _userBusiness.GetPageList(request);
            return new JsonResult(model, settings);
        }

        /// <summary>
        /// 获取某个用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<JsonResult> GetDetailInfo(string id)
        {
            var settings = new JsonSerializerSettings()
            {
                //ignore these properties because not used 
                ContractResolver = new CriteriaContractResolver(new List<string>()
                {
                    "Password"
                })
            };
            var userInfo = await _userBusiness.GetDetail(id);
            return new JsonResult(userInfo, settings);
        }

        /// <summary>
        /// 新增用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultModel<string>> AddUser([FromBody]AddUserRequest request)
        {
            return await _userBusiness.Insert(_mapper.Map<UserInfo>(request));
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResultModel<string>> UpdateUser([FromBody]UpdateUserRequest request)
        {
            return await _userBusiness.Update(_mapper.Map<UserInfo>(request));
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ResultModel<string>> DeleteUser(string id)
        {
            return await _userBusiness.Delete(id);
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("password")]
        public async Task<ResultModel<string>> UpdatePassword([FromBody]ChangePasswordRequest request)
        {
            return await _userBusiness.UpdatePassword(request);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("status")]
        public async Task<ResultModel<string>> UpdateStatus([FromBody]UpdateStatusRequest request)
        {
            return await _userBusiness.UpdateStatus(request);
        }

    }

    public class CriteriaContractResolver : DefaultContractResolver
    {
        readonly IEnumerable<string> _ignoreProperties;

        public CriteriaContractResolver(IEnumerable<string> ignoreProperties)
        {
            _ignoreProperties = ignoreProperties;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            property.ShouldSerialize = instance => !_ignoreProperties.Contains(property.PropertyName);
            return property;
        }
    }
}