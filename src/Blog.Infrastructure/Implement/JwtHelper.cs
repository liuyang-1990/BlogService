﻿using Blog.Model;
using Blog.Model.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace Blog.Infrastructure.Implement
{
    /// <summary>
    /// 生成Token，和解析Token
    /// </summary>
    public class JwtHelper : IJwtHelper
    {
        private readonly IOptions<JwtConfig> _jwtConfig;
        private readonly IRedisHelper _redisHelper;
        public JwtHelper(IOptions<JwtConfig> jwtConfig, IRedisHelper redisHelper)
        {
            _jwtConfig = jwtConfig;
            _redisHelper = redisHelper;
        }

        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <param name="isRefresh"></param>
        /// <returns></returns>
        public object IssueJwt(JwtToken tokenModel, bool isRefresh = false)
        {
            var dateTime = DateTime.UtcNow;
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti,tokenModel.Uid.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,dateTime.ToUniversalTime().ToString(),ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Iss, _jwtConfig.Value.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud,_jwtConfig.Value.Audience),
            };
            // 可以将一个用户的多个角色全部赋予
            claims.AddRange(tokenModel.Role.Split(",").Select(s => new Claim(ClaimTypes.Role, s)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Value.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                notBefore: dateTime,
                claims: claims,
                expires: dateTime.AddHours(2),
                signingCredentials: creds);
            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);
            var refreshToken = Guid.NewGuid().ToString().Replace("-", "");
            if (!isRefresh)
            {
                _redisHelper.Set($"refresh_token_{tokenModel.Uid}", refreshToken, TimeSpan.FromDays(15));
            }
            return new
            {
                access_token = encodedJwt,
                token_type = "Bearer",
                expire_in = 7200,
                refresh_token = refreshToken
            };
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <returns></returns>
        public JObject RefreshJwt(string refreshToken, JwtToken tokenModel)
        {
            if (!_redisHelper.ContainsKey($"refresh_token_{tokenModel.Uid}"))
            {
                return null;
            }
            var val = _redisHelper.Get<string>($"refresh_token_{tokenModel.Uid}");
            if (string.IsNullOrEmpty(val))
            {
                _redisHelper.Remove($"refresh_token_{tokenModel.Uid}");
                return null;
            }
            var tokenStr = JsonConvert.SerializeObject(IssueJwt(tokenModel, true));
            return JObject.Parse(tokenStr);
        }
    }
}