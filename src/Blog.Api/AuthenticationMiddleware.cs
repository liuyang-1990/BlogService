using Blog.Infrastructure;
using Blog.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Api
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IJwtHelper _jwtHelper;
        public AuthenticationMiddleware(RequestDelegate next, IJwtHelper jwtHelper)
        {
            _next = next;
            _jwtHelper = jwtHelper;
        }

        public Task Invoke(HttpContext httpContext)
        {

            if (!httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                return _next(httpContext);
            }
            var tokenHeader = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var jwtArr = tokenHeader.Split('.');
            //var header = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(jwtArr[0]));
            var payLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(jwtArr[1]));
            //验证是否在有效期内（也应该必须）
            var now = ToUnixEpochDate(DateTime.UtcNow);
            var success = (now >= long.Parse(payLoad["nbf"].ToString()) && now < long.Parse(payLoad["exp"].ToString()));
            if (success) return _next(httpContext);
            var token = _jwtHelper.RefreshJwt(tokenHeader, new JwtToken()
            {
                Uid = int.Parse(payLoad["jti"].ToString()),
                Role = payLoad[ClaimTypes.Role].ToString()
            });
            if (token == null)
            {
                return _next(httpContext);
            }
            httpContext.Request.Headers["Authorization"] = "Bearer " + token.Value<string>("access_token");
            httpContext.Response.Headers.Add("Authorization", token.Value<string>("access_token"));
            return _next(httpContext);
        }

        private long ToUnixEpochDate(DateTime date) =>
            (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}