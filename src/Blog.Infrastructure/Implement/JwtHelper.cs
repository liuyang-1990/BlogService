using Blog.Infrastructure.DI;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Infrastructure.Implement
{
    /// <summary>
    /// 生成Token，和解析Token
    /// </summary>
    [Injector(typeof(IJwtHelper), Lifetime = ServiceLifetime.Singleton)]
    public class JwtHelper : IJwtHelper
    {
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;
        public JwtHelper(IConfiguration configuration, IDistributedCache cache)
        {
            _configuration = configuration;
            _cache = cache;
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
            claims.AddRange(tokenModel.Role.ToString().Split(",").Select(s => new Claim(ClaimTypes.Role, s)));
            claims.Add(new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(tokenModel)));
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
            var refreshToken = GenerateRefreshToken();
            _cache.SetStringAsync($"refresh_token_{tokenModel.Uid}", refreshToken,
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(10)
                });
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

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

    }
}