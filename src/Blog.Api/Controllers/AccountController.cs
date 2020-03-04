using Blog.Business;
using Blog.Infrastructure;
using Blog.Infrastructure.Cryptography;
using Blog.Model;
using Blog.Model.Request.Account;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
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
        private readonly IDesEncrypt _desEncrypt;
        public AccountController(IJwtHelper jwtHelper,
            IUserBusiness userBusiness,
            IDesEncrypt desEncrypt)
        {
            _jwtHelper = jwtHelper;
            _userBusiness = userBusiness;
            _desEncrypt = desEncrypt;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequest loginRequest)
        {
            var userInfo = await _userBusiness.GetUserByUserName(loginRequest.UserName, loginRequest.Password);
            if (userInfo == null)
            {
                return BadRequest(new
                {
                    Msg = "Username or password is incorrect!"
                });
            }
            //用户被禁用
            if (!userInfo.IsActive)
            {
                return BadRequest(new
                {
                    Msg = "User is disabled, please contact administrator!"
                });
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid,_desEncrypt.Encrypt(userInfo.Id.ToString())),
                new Claim(ClaimTypes.NameIdentifier,userInfo.UserName),
                new Claim(ClaimTypes.Role,Enum.Parse(typeof(RoleDesc), userInfo.Role.ToString()).ToString())
            };
            var token = _jwtHelper.CreateAccessToken(claims);
            return Ok(new
            {
                AccessToken = token
            });
        }

    }
}