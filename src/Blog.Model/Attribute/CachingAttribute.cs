using System;

namespace Blog.Model
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CachingAttribute : Attribute
    {
        /// <summary>
        /// 缓存过期时间
        /// </summary>
        public int AbsoluteExpiration { get; set; } = 30;
    }
}