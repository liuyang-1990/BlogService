using Blog.Infrastructure.Cryptography;
using Blog.Infrastructure.DI;
using Blog.Infrastructure.Utility;
using System;

namespace Blog.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string RemovePostFix(this string str, params string[] postFixes)
        {
            if (str == null)
            {
                return null;
            }
            if (str == string.Empty)
            {
                return string.Empty;
            }
            if (postFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var postFix in postFixes)
            {
                if (str.EndsWith(postFix))
                {
                    return str.Left(str.Length - postFix.Length);
                }
            }
            return str;
        }

        public static string Left(this string str, int len)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(0, len);
        }


        public static string ToDecrypted(this string encryptedId)
        {
            if (string.IsNullOrEmpty(encryptedId))
            {
                return encryptedId;
            }
            var cryp = CoreContainer.Current.GetService<IDesEncrypt>();
            var utility = CoreContainer.Current.GetService<IUrlUtility>();
            return cryp.Decrypt(utility.UnTransCodeBase64(encryptedId));
        }

        public static string ToEncrypted(this string decryptedId)
        {
            var cryp = CoreContainer.Current.GetService<IDesEncrypt>();
            var utility = CoreContainer.Current.GetService<IUrlUtility>();
            return utility.TransCodeBase64(cryp.Encrypt(decryptedId));
        }
    }
}