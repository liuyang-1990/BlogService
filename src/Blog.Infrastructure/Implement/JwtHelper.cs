﻿using Blog.Model;
using Blog.Model.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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

        public JwtHelper(IOptions<JwtConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig;
        }
        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public string IssueJwt(JwtToken tokenModel)
        {
            var dateTime = DateTime.UtcNow;
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti,tokenModel.Uid.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,dateTime.ToString(),ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Iss, _jwtConfig.Value.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud,_jwtConfig.Value.Audience)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Value.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: dateTime.AddHours(1),
                signingCredentials: creds);
            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);
            return encodedJwt;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public JwtToken SerializeJwt(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(token);
            jwtToken.Payload.TryGetValue("Role", out object role);
            var tm = new JwtToken()
            {
                Uid = int.Parse(jwtToken.Id),
                Role = role?.ToString()
            };
            return tm;
        }
    }
}