using AspectCore.Injector;
using Blog.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace Blog.Infrastructure.Implement
{
    [Injector(typeof(IRedisHelper), LifeTime = Lifetime.Singleton)]
    public class RedisHelper : IRedisHelper
    {
        private readonly string _redisConnenctionString;
        private volatile ConnectionMultiplexer _redisConnection;
        private readonly object _redisConnectionLock = new object();
        private readonly ILogger<RedisHelper> _logger;
        public RedisHelper(IConfiguration configuration, ILogger<RedisHelper> logger)
        {
            _logger = logger;
            var redisConnenctionString = configuration["RedisCaching:ConnectionString"];
            if (string.IsNullOrWhiteSpace(redisConnenctionString))
            {
                throw new ArgumentException("redis config is empty", nameof(redisConnenctionString));
            }
            _redisConnenctionString = redisConnenctionString;
            _redisConnection = GetRedisConnection();    
        }

        private ConnectionMultiplexer GetRedisConnection()
        {
            //如果已经连接实例，直接返回
            if (_redisConnection != null && _redisConnection.IsConnected)
            {
                return _redisConnection;
            }
            //加锁，防止异步编程中，出现单例无效的问题
            lock (_redisConnectionLock)
            {
                if (_redisConnection != null)
                {
                    //释放redis连接
                    _redisConnection.Dispose();
                }
                try
                {
                    _redisConnection = ConnectionMultiplexer.Connect(_redisConnenctionString);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            return _redisConnection;
        }
        /// <summary>
        ///  获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            var value = _redisConnection.GetDatabase().StringGet(key);
            if (value.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default(T);
        }
        /// <summary>
        ///  获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            return _redisConnection.GetDatabase().StringGet(key);
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set<T>(string key, T value)
        {

            _redisConnection.GetDatabase().StringSet(key, JsonConvert.SerializeObject(value), TimeSpan.FromHours(1));

        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        public void Set<T>(string key, T value, TimeSpan timeSpan)
        {
            _redisConnection.GetDatabase().StringSet(key, JsonConvert.SerializeObject(value), timeSpan);
        }
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return _redisConnection.GetDatabase().KeyExists(key);
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            _redisConnection.GetDatabase().KeyDelete(key);
        }


    }
}