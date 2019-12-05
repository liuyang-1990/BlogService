using Blog.Infrastructure.DI;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Text.RegularExpressions;

namespace Blog.Infrastructure.Implement
{
    [Injector(typeof(IBlogUrlHelper), ServiceLifetime = ServiceLifetime.Scoped)]
    public class BlogUrlHelper : IBlogUrlHelper
    {

        private readonly Regex queryStringRegex = new Regex("([^?=&]+)(=([^&]*))?");
        /// <summary>
        /// 截取参数,取不到值时返回""
        /// </summary>
        /// <param name="url">不带?号的url参数</param>
        /// <param name="para">要取的参数</param>
        public string GetQueryString(string url, string para)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }
            url = url.Trim('?').Replace("%26", "&").Replace('?', '&');
            var originalQueryDic = queryStringRegex.Matches(url).ToDictionary(x => x.Groups[1].Value, x => x.Groups[3].Value);
            if (originalQueryDic.ContainsKey(para))
            {
                return originalQueryDic[para];
            }
            return null;
        }
    }
}
