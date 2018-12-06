using Blog.Infrastructure;
using Blog.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [BlogApiController]
    [EnableCors("allowAll")]//支持跨域
    public class LoginController : ControllerBase
    {

        private readonly IJwtHelper _jwtHelper;
        public LoginController(IJwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("token")]
        public JsonResult GetJwtStr()
        {
            var jwtStr = _jwtHelper.IssueJwt(new JwtToken()
            {
                Uid = "1",
                Role = "admin"
            });

            return new JsonResult(jwtStr);
        }
    }
}