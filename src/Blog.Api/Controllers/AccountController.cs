using Blog.Business;
using Blog.Infrastructure;
using Blog.Infrastructure.Extensions;
using Blog.Infrastructure.Implement;
using Blog.Model;
using Blog.Model.Db;
using Blog.Model.Request.Account;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Blog.Api.Controllers
{
    [ApiController]
    [Route("account")]
    [EnableCors("LimitRequests")]//支持跨域
    public class AccountController : ControllerBase
    {
        private readonly IJwtHelper _jwtHelper;
        private readonly IUserBusiness _userBusiness;
        private readonly IDistributedCache _distributedCache;
        public AccountController(IJwtHelper jwtHelper,
            IUserBusiness userBusiness,
            IDistributedCache distributedCache)
        {
            _jwtHelper = jwtHelper;
            _userBusiness = userBusiness;
            _distributedCache = distributedCache;
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
                new Claim(ClaimTypes.Sid,userInfo.Id.ToString().ToEncrypted()),
                new Claim(ClaimTypes.NameIdentifier,userInfo.UserName),
                new Claim(ClaimTypes.Role,Enum.Parse(typeof(RoleDesc), userInfo.Role.ToString()).ToString())
            };
            var token = _jwtHelper.CreateAccessToken(claims);
            return Ok(new
            {
                AccessToken = token,
                Expires = TimeSpan.FromDays(1).Days,
                UserName = userInfo.UserName
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequest register)
        {
            var captcha = await _distributedCache.GetStringAsync(register.Email);
            if (string.IsNullOrEmpty(captcha))
            {
                return BadRequest(new { Msg = "验证码已过期。" });
            }
            if (captcha != register.Captcha)
            {
                return BadRequest(new { Msg = "验证码不正确" });
            }
            var success = await _userBusiness.InsertAsync(TinyMapper.Map<UserInfo>(register));
            if (!success)
            {
                return BadRequest();
            }
            var userInfo = await _userBusiness.GetUserByUserName(register.UserName, register.Password);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid,userInfo.Id.ToString().ToEncrypted()),
                new Claim(ClaimTypes.NameIdentifier,userInfo.UserName),
                new Claim(ClaimTypes.Role,Enum.Parse(typeof(RoleDesc), userInfo.Role.ToString()).ToString())
            };
            var token = _jwtHelper.CreateAccessToken(claims);
            return Ok(new
            {
                AccessToken = token,
                Expires = TimeSpan.FromDays(1).Days,
                UserName = register.UserName
            });
        }

        [HttpGet("captcha")]
        public async Task<IActionResult> GetCaptcha(string to)
        {
            to = HttpUtility.UrlDecode(to);
            var captcha = RandomHelper.GetRandomNum(6);
            var body = $"<p style='font-size:14px;color:#333;line-height:30px;'>您本次的验证码为:</p><p><b style='font-size:18px;color:#f90;line-height:30px;'>{captcha}</b> <span style='font-size:14px;margin-left:10px;color:#979797;line-height:30px;'>(请在5分钟内完成验证。)</span><p>";
            await MailHelper.SendEMailAsync(to, "验证码", body);
            await _distributedCache.SetAsync(to, Encoding.UTF8.GetBytes(captcha), new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            return Ok(new
            {
                Captcha = captcha
            });
        }
    }
}