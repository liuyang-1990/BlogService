namespace Blog.Infrastructure.Cryptography
{
    public interface IMd5Helper
    {
        /// <summary>
        /// MD5加密,使用的UTF8编码
        /// </summary>
        /// <param name="originStr">待加密字串</param>
        /// <param name="length">16或32值之一</param>
        /// <returns></returns>
        string Encrypt(string originStr, int length = 32);
    }
}