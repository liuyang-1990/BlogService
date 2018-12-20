using Blog.Business;
using Blog.Model.Db;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [EnableCors("allowAll")]//支持跨域
    [BlogApiController]
    [Authorize(Policy = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;

        public UserController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        [HttpGet("page")]
        public async Task<JsonResultModel<UserInfo>> GetPageList(int pageIndex, int pageSize)
        {
            return await _userBusiness.GetPageList(pageIndex, pageSize, null);
        }

        [HttpGet("{id}")]
        public async Task<UserInfo> GetDetailInfo(int id)
        {
            return await _userBusiness.GetDetail(id);
        }

        [HttpPost]
        public async Task<BaseResponse> AddUser([FromBody]UserInfo user)
        {
            return await _userBusiness.Insert(user);
        }

        [HttpDelete("{id}")]
        public async Task<BaseResponse> DeleteUser(int id)
        {
            return await _userBusiness.Delete(id);
        }

        [HttpPut]
        public async Task<BaseResponse> UpdateUser([FromBody]UserInfo user)
        {
            return await _userBusiness.Update(user);
        }
    }
}