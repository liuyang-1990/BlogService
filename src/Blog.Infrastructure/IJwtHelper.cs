using Blog.Model;

namespace Blog.Infrastructure
{
    public interface IJwtHelper
    {
        string IssueJwt(JwtToken tokenModel);

        JwtToken SerializeJwt(string token);
    }
}