using Blog.Model;
using System.IdentityModel.Tokens.Jwt;

namespace Blog.Infrastructure.AuthHelp
{
    /// <summary>
    /// 生成Token，和解析Token
    /// </summary>
    public class JwtHelper
    {


        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static JwtToken SerializeJwt(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(token);
           // jwtToken.Payload.TryGetValue("sub",object )
            return null;
        }
    }
}