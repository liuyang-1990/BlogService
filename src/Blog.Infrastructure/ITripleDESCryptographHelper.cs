namespace Blog.Infrastructure
{
    public interface ITripleDESCryptographHelper
    {
        string Encrypt(string plainText);

        string Decrypt(string encryptedBase64String);
    }
}