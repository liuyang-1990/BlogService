using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Infrastructure
{
    public interface IUrlHelper
    {
        string GetQueryString(string url, string para);
    }
}
