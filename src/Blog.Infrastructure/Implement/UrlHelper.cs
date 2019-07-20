using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Blog.Infrastructure.Implement
{
    public class UrlHelper : IUrlHelper
    {
        /// <summary>
        /// 截取参数,取不到值时返回""
        /// </summary>
        /// <param name="url">不带?号的url参数</param>
        /// <param name="para">要取的参数</param>
        public string GetQueryString(string url, string para)
        {
            if (string.IsNullOrEmpty(url))
            {
                return url;
            }
            url = url.Trim('?').Replace("%26", "&").Replace('?', '&');
            int num = url.Length;
            for (int i = 0; i < num; i++)
            {
                int startIndex = i;
                int num4 = -1;
                while (i < num)
                {
                    char ch = url[i];
                    if (ch == '=')
                    {
                        if (num4 < 0)
                        {
                            num4 = i;
                        }
                    }
                    else if (ch == '&')
                    {
                        break;
                    }
                    i++;
                }
                string str = null;
                string str2 = null;
                if (num4 >= 0)
                {
                    str = url.Substring(startIndex, num4 - startIndex);
                    str2 = url.Substring(num4 + 1, (i - num4) - 1);
                    if (str == para)
                    {
                        return HttpUtility.UrlDecode(str2);
                    }
                }
            }
            return "";
        }
    }
}
