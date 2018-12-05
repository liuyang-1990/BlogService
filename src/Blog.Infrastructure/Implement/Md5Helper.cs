using System.Security.Cryptography;
using System.Text;

namespace Blog.Infrastructure.Implement
{
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
                sBuilder.Append(t.ToString("x"));
            }
            return sBuilder.ToString();
        }
    }
}