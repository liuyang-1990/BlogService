using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Infrastructure
{
    public interface IBlogUrlHelper
    {
        string GetQueryString(string url, string para);
    }
}
