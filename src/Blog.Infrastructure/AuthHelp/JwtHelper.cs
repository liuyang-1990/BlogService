using System;
using Blog.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blog.Model.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Infrastructure.AuthHelp
{
    /// <summary>
    /// 生成Token，和解析Token
    /// </summary>
    public class JwtHelper
    {
        public static IConfiguration Configuration;

        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string IssueJwt(JwtToken tokenModel)
        {
            var dateTime = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti,tokenModel.Uid),
                new Claim("Role",tokenModel.Role),
                new Claim(JwtRegisteredClaimNames.Iat,dateTime.ToString(),ClaimValueTypes.Integer64),
            };
            var jwtConfig = Configuration.GetSection("JwtAuth");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                issuer: jwtConfig["Issuer"],
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
        public static JwtToken SerializeJwt(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(token);
            jwtToken.Payload.TryGetValue("Role", out object role);
            var tm = new JwtToken()
            {
                Uid = jwtToken.Id,
                Role = role?.ToString()
            };

            return tm;
        }
    }
}