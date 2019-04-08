using Blog.Model;
using Newtonsoft.Json.Linq;

namespace Blog.Infrastructure
{
    public interface IJwtHelper
    {
        object IssueJwt(JwtToken tokenModel, bool isRefresh = false);

        JObject RefreshJwt(string refreshToken, JwtToken tokenModel);
    }
}