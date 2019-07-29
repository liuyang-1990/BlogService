using System;

namespace Blog.Model
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CachingAttribute : Attribute
    {
        public CachingAttribute()
        {

        }
        public CachingAttribute(string cacheKey)
        {
            this.CacheKey = cacheKey;
        }
        public string CacheKey { get; set; }
        /// <summary>
        /// 缓存过期时间
        /// </summary>
        public int AbsoluteExpiration { get; set; } = 30;
    }
}