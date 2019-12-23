using Blog.Infrastructure.DI;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Infrastructure.Cryptography
{
    /// <summary>
    /// 不可逆加密
    /// 1 防止被篡改
    /// 2 防止明文存储
    /// 3 防止抵赖，数字签名
    /// </summary>
    [Injector(typeof(IMd5Helper), Lifetime = ServiceLifetime.Scoped)]
    public class Md5Helper : IMd5Helper
    {
        /// <summary>
        /// MD5加密,使用的UTF8编码
        /// </summary>
        /// <param name="originStr">待加密字串</param>
        /// <param name="length">16或32值之一</param>
        /// <returns></returns>
        public string Encrypt(string originStr, int length = 32)
        {
            if (string.IsNullOrEmpty(originStr))
            {
                return null;
            }
            var md5Hasher = new MD5CryptoServiceProvider();
            var data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(originStr));
            var sBuilder = new StringBuilder();
            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return length == 16 ? sBuilder.ToString().Substring(8, 16)
                : sBuilder.ToString();
        }
    }
}