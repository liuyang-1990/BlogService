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
    [ApiController]
    [ApiVersion("1.0")]
    [BlogApiController]
    [EnableCors("LimitRequests")]//支持跨域
    public class AccountController : ControllerBase
    {
        private readonly IJwtHelper _jwtHelper;
        private readonly IUserBusiness _userBusiness;
        public AccountController(IJwtHelper jwtHelper,
            IUserBusiness userBusiness)
        {
            _jwtHelper = jwtHelper;
            _userBusiness = userBusiness;
        }

        [HttpPost("login")]
        public async Task<ResultModel<LoginResponse>> Login([FromBody]LoginRequest loginRequest)
        {
            var response = new ResultModel<LoginResponse>();
            var userInfo = await _userBusiness.GetUserByUserName(loginRequest.UserName, loginRequest.Password);
            if (userInfo == null)
            {
                response.IsSuccess = false;
                response.Status = "1";
                return response;
            }
            //用户被禁用
            if (userInfo.Status == 0)
            {
                response.IsSuccess = false;
                response.Status = "2";
                return response;
            }
            var loginResponse = _jwtHelper.IssueJwt(new JwtToken()
            {
                Uid = userInfo.Id,
                UserName = userInfo.UserName,
                Avatar = userInfo.Avatar,
                Role = Enum.Parse(typeof(RoleDesc), userInfo.Role.ToString()).ToString()
            });
            response.IsSuccess = true;
            response.Status = "0";
            response.ResultInfo = loginResponse;
            return response;
        }

    }
}