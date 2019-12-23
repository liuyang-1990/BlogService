using Blog.Infrastructure.DI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Infrastructure.Cryptography
{
    /// <summary>
    /// 对称加密算法的优点是速度快，
    /// 缺点是密钥管理不方便，要求共享密钥。
    /// 可逆对称加密  密钥长度8
    /// </summary>
    [Injector(typeof(IDesEncrypt), Lifetime = ServiceLifetime.Scoped)]
    public class DesEncrypt : IDesEncrypt
    {
        private static readonly byte[] _rgbKey = Encoding.ASCII.GetBytes(AppConst.DesKey.Substring(0, 8));
        private static readonly byte[] _rgbIV = Encoding.ASCII.GetBytes(AppConst.DesKey.Insert(0, "w").Substring(0, 8));

        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="text">需要加密的值</param>
        /// <returns>加密后的结果</returns>
        public string Encrypt(string text)
        {
            var dsp = new DESCryptoServiceProvider();
            using var memStream = new MemoryStream();
            var stream = new CryptoStream(memStream, dsp.CreateEncryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write);
            var sWriter = new StreamWriter(stream);
            sWriter.Write(text);
            sWriter.Flush();
            stream.FlushFinalBlock();
            memStream.Flush();
            return Convert.ToBase64String(memStream.GetBuffer(), 0, (int)memStream.Length);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="encryptText">加密的值</param>
        /// <returns>解密后的结果</returns>
        public string Decrypt(string encryptText)
        {
            var dsp = new DESCryptoServiceProvider();
            var buffer = Convert.FromBase64String(encryptText);
            using var memStream = new MemoryStream();
            var stream = new CryptoStream(memStream, dsp.CreateDecryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write);
            stream.Write(buffer, 0, buffer.Length);
            stream.FlushFinalBlock();
            return Encoding.UTF8.GetString(memStream.ToArray());
        }
    }
}