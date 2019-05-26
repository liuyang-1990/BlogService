using System;

namespace Blog.Model.Attribute
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CachingAttribute : System.Attribute
    {
        /// <summary>
        /// 缓存过期时间
        /// </summary>
        public int AbsoluteExpiration { get; set; } = 30;
    }
}