using AspectCore.Injector;
using Blog.Model;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Infrastructure.Implement
{
    [Injector(typeof(IMd5Helper), LifeTime = Lifetime.Scoped)]
    public class Md5Helper : IMd5Helper
    {
        public string Encrypt32(string originStr)
        {
            if (originStr == null)
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
            return sBuilder.ToString();
        }
    }
}