using System.Collections.Generic;
using System.Security.Claims;

namespace Blog.Infrastructure
{
    public interface IJwtHelper
    {
        string CreateAccessToken(IEnumerable<Claim> claims);
    }
}