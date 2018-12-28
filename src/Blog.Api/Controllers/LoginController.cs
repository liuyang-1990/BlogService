using Blog.Business;
using Blog.Infrastructure;
using Blog.Model;
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
        [HttpGet("token")]
        public async Task<ResultModel<string>> GetJwtStr(string userName, string password)
        {
            var response = new ResultModel<string>();
            var userInfo = await _userBusiness.GetUserByUserName(userName, password);
            if (userInfo == null)
            {
                response.Code = (int)ResponseStatus.Fail;
                response.Msg = MessageConst.LoginFail;
                response.ResultInfo = string.Empty;
                return response;
            }
            userInfo.LastLoginTime = DateTime.Now;
            await _userBusiness.Update(userInfo);
            var jwtStr = _jwtHelper.IssueJwt(new JwtToken()
            {
                Uid = userInfo.Id,
                Role = Enum.Parse(typeof(RoleDesc), userInfo.Role.ToString()).ToString()
            });
            response.Code = (int)ResponseStatus.Ok;
            response.Msg = MessageConst.Ok;
            response.ResultInfo = jwtStr;
            return response;
        }
    }
}