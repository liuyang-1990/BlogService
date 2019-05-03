using Blog.Model;
using Blog.Model.Response;

namespace Blog.Infrastructure
{
    public interface IJwtHelper
    {
        LoginResponse IssueJwt(JwtToken tokenModel, bool isRefresh = false);

        string RefreshJwt(string refreshToken, JwtToken tokenModel);
    }
}