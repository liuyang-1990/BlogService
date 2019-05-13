using Blog.Model;
using Blog.Model.Response;

namespace Blog.Infrastructure
{
    public interface IJwtHelper
    {
        string SecurityKey { get; set; }
        LoginResponse IssueJwt(JwtToken tokenModel, bool isRefresh = false);

        string RefreshJwt(string refreshToken, JwtToken tokenModel);
    }
}