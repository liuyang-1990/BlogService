using AutoMapper;
using Blog.Business;
using Blog.Model.Db;
using Blog.Model.Request.User;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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
            return await _userBusiness.GetDetail(id);
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