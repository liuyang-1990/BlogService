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
    public class AccountController : ControllerBase
    {
        private readonly IJwtHelper _jwtHelper;
        private readonly IUserBusiness _userBusiness;
        private readonly ITripleDESCryptographHelper _cryptographHelper;
        public AccountController(IJwtHelper jwtHelper, IUserBusiness userBusiness, ITripleDESCryptographHelper cryptographHelper)
        {
            _jwtHelper = jwtHelper;
            _userBusiness = userBusiness;
            _cryptographHelper = cryptographHelper;
        }

        [HttpPost("login")]
        public async Task<ResultModel<LoginResponse>> Login(LoginRequest loginRequest)
        {
            var response = new ResultModel<LoginResponse>();
            var userInfo = await _userBusiness.GetUserByUserName(loginRequest.UserName, loginRequest.Password);
            if (userInfo == null)
            {
                response.IsSuccess = false;
                response.Status = "-1";
                return response;
            }
            var loginResponse = _jwtHelper.IssueJwt(new JwtToken()
            {
                Uid = userInfo.Id,
                Role = Enum.Parse(typeof(RoleDesc), userInfo.Role.ToString()).ToString()
            });
            response.IsSuccess = true;
            response.Status = "0";
            loginResponse.UserName = userInfo.UserName;
            loginResponse.UserId = _cryptographHelper.Encrypt(userInfo.Id.ToString());
            response.ResultInfo = loginResponse;
            return response;
        }

    }
}