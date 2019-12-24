namespace Blog.Infrastructure.Utility
{
    public interface IUrlUtility
    {
        string TransCodeBase64(string base64Code);
        string UnTransCodeBase64(string transBase64Code);
    }
}