using Blog.Model;

namespace Blog.Infrastructure
{
    public interface IJwtHelper
    {
        object IssueJwt(JwtToken tokenModel);

        object RefreshJwt(string refreshToken, JwtToken tokenModel);
    }
}