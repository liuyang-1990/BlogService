using Blog.Infrastructure.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    [Injector(typeof(IJwtHelper), Lifetime = ServiceLifetime.Singleton)]
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration _configuration;
        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 生成AccessToken
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public string CreateAccessToken(IEnumerable<Claim> claims)
        {
            var jwtClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };
            jwtClaims.AddRange(claims);
            var now = DateTime.UtcNow;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:JwtBearer:SecurityKey"]));
            var jwt = new JwtSecurityToken(
                issuer: _configuration["Authentication:JwtBearer:Issuer"],
                _configuration["Authentication:JwtBearer:Audience"],
                claims: jwtClaims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromDays(1)),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}