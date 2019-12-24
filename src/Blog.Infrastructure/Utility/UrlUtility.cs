using Blog.Infrastructure.DI;

namespace Blog.Infrastructure.Utility
{
    [Injector(typeof(IUrlUtility))]
    public class UrlUtility : IUrlUtility
    {
        public string TransCodeBase64(string base64Code)
        {
            return base64Code?.Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }
        public string UnTransCodeBase64(string transBase64Code)
        {
            if (transBase64Code == null)
            {
                return null;
            }
            transBase64Code = transBase64Code.Replace('-', '+').Replace('_', '/');
            switch (transBase64Code.Length % 4)
            {
                case 2:
                    transBase64Code += "==";
                    break;
                case 3:
                    transBase64Code += "=";
                    break;
            }
            return transBase64Code;
        }
    }
}