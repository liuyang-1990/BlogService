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
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="user">过滤条件</param>
        /// <returns></returns>
        [HttpGet("page")]
        public async Task<JsonResultModel<UserInfo>> GetPageList(int pageIndex, int pageSize, UserRequest user)
        {
            return await _userBusiness.GetPageList(pageIndex, pageSize, user);
        }

        /// <summary>
        /// 获取某个用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<UserInfo> GetDetailInfo(int id)
        {
            return await _userBusiness.GetDetail(id);
        }

        /// <summary>
        /// 新增用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> AddUser([FromBody]UserRequest user)
        {
            var userInfo = _mapper.Map<UserInfo>(user);
            return await _userBusiness.Insert(userInfo);
        }

        [HttpDelete("{id}")]
        public async Task<BaseResponse> DeleteUser(int id)
        {
            return await _userBusiness.Delete(id);
        }

        [HttpPut]
        public async Task<BaseResponse> UpdateUser([FromBody]UserRequest user)
        {
            var userInfo = _mapper.Map<UserInfo>(user);
            return await _userBusiness.Update(userInfo);
        }
    }
}