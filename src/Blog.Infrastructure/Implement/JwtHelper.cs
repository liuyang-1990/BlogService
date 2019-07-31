using AspectCore.Injector;
using Blog.Model;
using Blog.Model.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;


namespace Blog.Infrastructure.Implement
{
    /// <summary>
    /// 生成Token，和解析Token
    /// </summary>
    [Injector(typeof(IJwtHelper), LifeTime = Lifetime.Singleton)]
    public class JwtHelper : IJwtHelper
    {
        private readonly IDistributedCache _cache;
        //  private readonly IRedisHelper _redisHelper;
        private readonly IConfiguration _configuration;
        public JwtHelper(IConfiguration configuration,
            IDistributedCache cache
            //IRedisHelper redisHelper
            )
        {
            _configuration = configuration;
            _cache = cache;
            // _redisHelper = redisHelper;
        }


        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <param name="isRefresh"></param>
        /// <returns></returns>
        public LoginResponse IssueJwt(JwtToken tokenModel, bool isRefresh = false)
        {
            var dateTime = DateTime.UtcNow;
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti,tokenModel.Uid.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,dateTime.ToUniversalTime().ToString(),ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration["JwtAuth:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Aud,_configuration["JwtAuth:Audience"]),
            };
            // 可以将一个用户的多个角色全部赋予
            claims.AddRange(tokenModel.Role.Split(",").Select(s => new Claim(ClaimTypes.Role, s)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtAuth:SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                notBefore: dateTime,
                claims: claims,
                expires: dateTime.AddHours(2),
                signingCredentials: creds);
            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);
            if (isRefresh)
            {
                return new LoginResponse()
                {
                    AccessToken = "Bearer " + encodedJwt,
                };
            }
            var refreshToken = Guid.NewGuid().ToString().Replace("-", "");
            _cache.SetStringAsync($"refresh_token_{tokenModel.Uid}", refreshToken,
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(15)
                });
            // _redisHelper.Set($"refresh_token_{tokenModel.Uid}", refreshToken, TimeSpan.FromDays(15));
            return new LoginResponse()
            {
                AccessToken = "Bearer " + encodedJwt,
                RefreshToken = refreshToken
            };
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <returns></returns>
        public string RefreshJwt(string refreshToken, JwtToken tokenModel)
        {
            var value = _cache.GetString($"refresh_token_{tokenModel.Uid}");
            if (!string.Equals(refreshToken, value, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            var loginResponse = IssueJwt(tokenModel, true);
            return loginResponse.AccessToken;
        }
    }
}