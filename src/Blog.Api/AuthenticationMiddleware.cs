using Blog.Infrastructure;
using Blog.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
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
            if (jwtArr.Length != 3)
            {
                return _next(httpContext);
            }
            //首先验证签名是否正确
            var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(_jwtHelper.SecurityKey));
            var success = string.Equals(jwtArr[2],
                Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(jwtArr[0], ".", jwtArr[1])))));
            if (!success)
            {
                //签名不正确
                return _next(httpContext);
            }
            var payLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(jwtArr[1]));
            //其次验证是否在有效期内
            var now = ToUnixEpochDate(DateTime.UtcNow);
            success = (now >= long.Parse(payLoad["nbf"].ToString()) && now < long.Parse(payLoad["exp"].ToString()));
            if (success) return _next(httpContext);
            var refreshToken = httpContext.Request.Headers["refresh_token"].ToString();
            var token = _jwtHelper.RefreshJwt(refreshToken, new JwtToken()
            {
                Uid = int.Parse(payLoad["jti"].ToString()),
                Role = payLoad[ClaimTypes.Role].ToString()
            });
            if (token == null)
            {
                return _next(httpContext);
            }
            httpContext.Request.Headers["Authorization"] = token;
            //在使用CORS方式跨域时，浏览器只会返回以下默认头部header:  
            // 1.Content-Language 2. Content - Type 3. Expires     4.Last - Modified  5. Pragma
            //在客户端获取自定义的header信息，需要在服务器端header中添加Access-Control-Expose-Headers
            httpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Authorization");
            httpContext.Response.Headers.Add("Authorization", token);
            return _next(httpContext);
        }

        private long ToUnixEpochDate(DateTime date) =>
            (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}