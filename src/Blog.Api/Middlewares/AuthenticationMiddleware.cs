using Blog.Infrastructure;
using Blog.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IJwtHelper _jwtHelper;

        private readonly IConfiguration _configuration;
        public AuthenticationMiddleware(RequestDelegate next,
            IJwtHelper jwtHelper,
           IConfiguration configuration)
        {
            _next = next;
            _jwtHelper = jwtHelper;
            _configuration = configuration;
        }

        public Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                return _next(httpContext);
            }
            var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var jwtHandler = new JwtSecurityTokenHandler();
            try
            {
                //验证Token
                jwtHandler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidIssuer = _configuration["JwtAuth:Issuer"],
                    ValidAudience = _configuration["JwtAuth:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtAuth:SecurityKey"])),
                    RequireSignedTokens = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out var _);

            }
            catch (SecurityTokenExpiredException)
            {
                //说明token过期
                var securityToken = jwtHandler.ReadJwtToken(token);
                var claims = securityToken.Payload.Claims;
                var userData = claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData);
                if (userData == null)
                {
                    return _next(httpContext);
                }
                //此时刷新token
                var refreshToken = httpContext.Request.Headers["x-refresh-token"].ToString();
                var newToken = _jwtHelper.RefreshJwt(refreshToken, JsonConvert.DeserializeObject<JwtToken>(userData.Value));
                if (newToken == null)
                {
                    return _next(httpContext);
                }
                httpContext.Request.Headers["Authorization"] = newToken;
                httpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Authorization");
                httpContext.Response.Headers.Add("Authorization", newToken);
            }
            catch
            {
                //其他异常，啥也不做
            }
            return _next(httpContext);
        }
    }
}