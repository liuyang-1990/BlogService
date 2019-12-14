
namespace Blog.Infrastructure
{
    public interface IJwtHelper
    {
        LoginResponse IssueJwt(JwtToken tokenModel, bool isRefresh = false);

        string RefreshJwt(string refreshToken, JwtToken tokenModel);
    }
}