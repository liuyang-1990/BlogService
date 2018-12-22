using System;

namespace Blog.Infrastructure
{
    public interface IRedisHelper
    {
        T Get<T>(string key);
        //设置
        void Set<T>(string key, T value);

        void Set<T>(string key, T value, TimeSpan timeSpan);

        bool ContainsKey(string key);

        //移除
        void Remove(string key);



    }
}