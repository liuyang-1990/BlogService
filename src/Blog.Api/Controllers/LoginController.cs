using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Infrastructure.AuthHelp;
using Blog.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [BlogApiController]
    [EnableCors("allowAll")]//支持跨域
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("token")]
        public JsonResult GetJwtStr()
        {
            var jwtStr = JwtHelper.IssueJwt(new JwtToken()
            {
                Uid = "1",
                Role = "admin"
            });

            return new JsonResult(jwtStr);
        }
    }
}