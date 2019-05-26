using Blog.Model.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
        public RedisHelper(IOptions<RedisConn> redisConn)
        {
            var redisConnenctionString = redisConn.Value.ConnectionString;
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
                this._redisConnection = ConnectionMultiplexer.Connect(_redisConnenctionString);
            }
            return this._redisConnection;
        }

        public T Get<T>(string key)
        {
            return JsonConvert.DeserializeObject<T>(_db.StringGet(key));
        }

        public string Get(string key)
        {
            return _db.StringGet(key);
        }

        public void Set<T>(string key, T value)
        {

            _db.StringSet(key, JsonConvert.SerializeObject(value), TimeSpan.FromHours(1));

        }
        public void HashSet<T>(string key, string subkey, T value, TimeSpan timeSpan)
        {
            _db.HashSet(key, new[]
            {
                new HashEntry(subkey, JsonConvert.SerializeObject(value)),
            });
            _db.KeyExpire(key, timeSpan);
        }
        public void Set<T>(string key, T value, TimeSpan timeSpan)
        {
            _db.StringSet(key, JsonConvert.SerializeObject(value), timeSpan);
        }

        public bool ContainsKey(string key)
        {
            return _db.KeyExists(key);
        }

        public void Remove(string key)
        {
            _db.KeyDelete(key);
        }


    }
}