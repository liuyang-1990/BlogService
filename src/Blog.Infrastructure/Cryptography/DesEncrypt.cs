using Blog.Infrastructure.DI;
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
    [Injector(typeof(IDesEncrypt))]
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
            var mStream = new MemoryStream(200);
            mStream.SetLength(0);
            // Convert the passed string to a byte array.
            var toEncrypt = Encoding.UTF8.GetBytes(text);
            var dsp = new DESCryptoServiceProvider();

            var cStream = new CryptoStream(mStream, dsp.CreateEncryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write);
            // Write the byte array to the crypto stream and flush it.
            cStream.Write(toEncrypt, 0, toEncrypt.Length);
            cStream.FlushFinalBlock();

            mStream.Flush();
            mStream.Seek(0, SeekOrigin.Begin);

            var ret = new byte[mStream.Length];
            mStream.Read(ret, 0, ret.Length);
            // Close the streams.
            cStream.Close();
            mStream.Close();
            return Convert.ToBase64String(ret, 0, ret.Length);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="encryptText">加密的值</param>
        /// <returns>解密后的结果</returns>
        public string Decrypt(string encryptText)
        {
            // Create the file streams to handle the input and output files.
            var fout = new MemoryStream(200);
            fout.SetLength(0);

            // Create variables to help with read and write.
            var bin = Convert.FromBase64String(encryptText);
            var des = new DESCryptoServiceProvider();
            var encStream = new CryptoStream(fout, des.CreateDecryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write);
            encStream.Write(bin, 0, bin.Length);
            encStream.FlushFinalBlock();
            fout.Flush();
            fout.Seek(0, SeekOrigin.Begin);

            // read all string
            var bout = new byte[fout.Length];
            fout.Read(bout, 0, bout.Length);
            encStream.Close();
            fout.Close();
            return Encoding.UTF8.GetString(bout);
        }
    }
}