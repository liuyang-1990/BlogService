﻿using Blog.Business;
using Blog.Infrastructure;
using Blog.Infrastructure.Extensions;
using Blog.Model;
using Blog.Model.Request.Account;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.Model.Db;
using Nelibur.ObjectMapper;


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
                AccessToken = token
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequest register)
        {
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
                AccessToken = token
            });
        }
    }
}