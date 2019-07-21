using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using StackExchange.Redis;
using System;

namespace Blog.Infrastructure.Implement
{
    public class RedisHelper : IRedisHelper
    {
        private readonly string _redisConnenctionString;
        private volatile ConnectionMultiplexer _redisConnection;
        private readonly object _redisConnectionLock = new object();
        private readonly IDatabase _db;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public RedisHelper(IConfiguration configuration)
        {

            var redisConnenctionString = configuration["RedisCaching:ConnectionString"];
            if (string.IsNullOrWhiteSpace(redisConnenctionString))
            {
                throw new ArgumentException("redis config is empty", nameof(redisConnenctionString));
            }
            this._redisConnenctionString = redisConnenctionString;
            this._redisConnection = GetRedisConnection();
            _db = this._redisConnection.GetDatabase();
        }

        private ConnectionMultiplexer GetRedisConnection()
        {
            //如果已经连接实例，直接返回
            if (this._redisConnection != null && this._redisConnection.IsConnected)
            {
                return this._redisConnection;
            }
            //加锁，防止异步编程中，出现单例无效的问题
            lock (_redisConnectionLock)
            {
                if (this._redisConnection != null)
                {
                    //释放redis连接
                    this._redisConnection.Dispose();
                }
                try
                {
                    this._redisConnection = ConnectionMultiplexer.Connect(_redisConnenctionString);
                }
                catch (Exception ex)
                {
                    _logger.Warn(ex.ToString);
                }
            }
            return this._redisConnection;
        }
        /// <summary>
        ///  获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            var value = _db.StringGet(key);
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
            return _db.StringGet(key);
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set<T>(string key, T value)
        {

            _db.StringSet(key, JsonConvert.SerializeObject(value), TimeSpan.FromHours(1));

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
            _db.StringSet(key, JsonConvert.SerializeObject(value), timeSpan);
        }
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return _db.KeyExists(key);
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            _db.KeyDelete(key);
        }


    }
}