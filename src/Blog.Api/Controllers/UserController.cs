using AutoMapper;
using Blog.Business;
using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [EnableCors("LimitRequests")]//支持跨域
    [BlogApiController]
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
        /// <param name="searchParams"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("page")]
        public async Task<JsonResultModel<UserInfoBase>> GetPageList(UserRequest searchParams, GridParams param)
        {
            var userInfos = await _userBusiness.GetPageList(searchParams, param);
            return _mapper.Map<JsonResultModel<UserInfoBase>>(userInfos);
        }

        /// <summary>
        /// 获取某个用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<UserInfoBase> GetDetailInfo(int id)
        {
            var user = await _userBusiness.GetDetail(id);
            return _mapper.Map<UserInfoBase>(user);
        }

        /// <summary>
        /// 新增用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultModel<string>> AddUser([FromBody]UserRequest user)
        {
            var userInfo = _mapper.Map<UserInfo>(user);
            return await _userBusiness.Insert(userInfo);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ResultModel<string>> DeleteUser(int id)
        {
            return await _userBusiness.Delete(id);
        }
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResultModel<string>> UpdateUser([FromBody]UserRequest user)
        {
            var userInfo = _mapper.Map<UserInfo>(user);
            return await _userBusiness.Update(userInfo);
        }
    }
}