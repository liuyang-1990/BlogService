using Blog.Business;
using Blog.Infrastructure;
using Blog.Model;
using Blog.Model.Request;
using Blog.Model.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [BlogApiController]
    [EnableCors("LimitRequests")]//支持跨域
    public class LoginController : ControllerBase
    {
        private readonly IJwtHelper _jwtHelper;
        private readonly IUserBusiness _userBusiness;

        public LoginController(IJwtHelper jwtHelper, IUserBusiness userBusiness)
        {
            _jwtHelper = jwtHelper;
            _userBusiness = userBusiness;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("token")]
        public async Task<object> GetJwtStr([FromBody]UserRequest userRequest)
        {
            var response = new ResultModel<string>();
            var userInfo = await _userBusiness.GetUserByUserName(userRequest.UserName, userRequest.Password);
            if (userInfo == null)
            {
                response.Code = (int)ResponseStatus.Fail;
                response.Msg = MessageConst.LoginFail;
                response.ResultInfo = string.Empty;
                return response;
            }
            userInfo.LastLoginTime = DateTime.Now;
            var res = await _userBusiness.Update(userInfo);
            if (res.Code != (int)ResponseStatus.Ok)
            {
                return res;
            }
            var jwtStr = _jwtHelper.IssueJwt(new JwtToken()
            {
                Uid = userInfo.Id,
                Role = Enum.Parse(typeof(RoleDesc), userInfo.Role.ToString()).ToString()
            });

            return jwtStr;
        }
    }
}