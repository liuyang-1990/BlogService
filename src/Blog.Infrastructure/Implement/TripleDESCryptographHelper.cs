using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Infrastructure.Implement
{
    public class TripleDESCryptographHelper : ITripleDESCryptographHelper
    {
        private const string Key = "P2IXwigbk+vVYUFM4yDAiu0k";
                                    
        public string Encrypt(string plainText)
        {
            // Create a MemoryStream.
            var mStream = new MemoryStream();
            var tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(Key),
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.ECB
            };
            //补位
            //CipherMode.CBC
            var cStream = new CryptoStream(mStream,
                tripleDESCryptoServiceProvider.CreateEncryptor(),
                CryptoStreamMode.Write);

            // Convert the passed string to a byte array.
            var toEncrypt = Encoding.UTF8.GetBytes(plainText);

            // Write the byte array to the crypto stream and flush it.
            cStream.Write(toEncrypt, 0, toEncrypt.Length);
            cStream.FlushFinalBlock();

            // Get an array of bytes from the
            // MemoryStream that holds the
            // encrypted data.
            var ret = mStream.ToArray();

            // Close the streams.
            cStream.Close();
            mStream.Close();

            // Return the encrypted buffer.
            return Convert.ToBase64String(ret).Replace('/', ' ');

        }


        public string Decrypt(string encryptedBase64String)
        {
            var inputByteArray = Convert.FromBase64String(encryptedBase64String.Replace(' ', '/'));
            // Create a new MemoryStream using the passed
            // array of encrypted data.
            var msDecrypt = new MemoryStream(inputByteArray);

            // Create a CryptoStream using the MemoryStream
            var tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(Key),
                Padding = PaddingMode.PKCS7,//补位
                Mode = CipherMode.ECB//CipherMode.CBC
            };
            var csDecrypt = new CryptoStream(msDecrypt,
                tripleDESCryptoServiceProvider.CreateDecryptor(),
                CryptoStreamMode.Read);

            // Create buffer to hold the decrypted data.
            var fromEncrypt = new byte[inputByteArray.Length];

            // Read the decrypted data out of the crypto stream
            // and place it into the temporary buffer.
            csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

            //Convert the buffer into a string and return it.
            return Encoding.UTF8.GetString(fromEncrypt).TrimEnd('\0');
        }


    }
}