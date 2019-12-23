namespace Blog.Infrastructure.Cryptography
{
    public interface IDesEncrypt
    {
        string Encrypt(string text);

        string Decrypt(string encryptText);
    }
}