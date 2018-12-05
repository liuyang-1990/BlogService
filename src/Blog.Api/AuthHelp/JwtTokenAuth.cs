using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Blog.Infrastructure.AuthHelp;

namespace Blog.Api.AuthHelp
{
    /// <summary>
    /// 中间件JwtTokenAuth，验证作用
    /// </summary>
    public class JwtTokenAuth
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="next"></param>
        public JwtTokenAuth(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 过滤请求
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                return _next(httpContext);
            }

            var tokenHeader = httpContext.Request.Headers["Authorization"].ToString();
            tokenHeader = tokenHeader.Substring("Bearer ".Length).Trim();
            var  tm = JwtHelper.SerializeJwt(tokenHeader);//解析Token
            //授权
            var claimList = new List<Claim>();
            var claim = new Claim(ClaimTypes.Role, tm.Role);//一个新声明  用角色
            claimList.Add(claim);
            var identity = new ClaimsIdentity(claimList);
            var principal = new ClaimsPrincipal(identity);//声明主体
            httpContext.User = principal;

            return _next(httpContext);
        }
    }
}