using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomHeaderMiddleware.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace CustomHeaderMiddleware.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IDatabase Database;
        protected readonly ICacheClient CacheClient;
        protected ISerializer Serializer;
        private static ConfigurationOptions ConfigurationOptions;

        private static readonly Lazy<ConnectionMultiplexer> LazyConnection
                = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(ConfigurationOptions));

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return LazyConnection.Value;
            }
        }

        public BaseController(ISerializer serializer, IOptions<RedisConfiguration> config)
        {
            Serializer = serializer;
            ConfigurationOptions = config.Value.ConfigurationOptions;
            CacheClient = new StackExchangeRedisCacheClient(Connection, Serializer);
            Database = Connection.GetDatabase();
        }

        ~BaseController()
        {
            Database.Multiplexer.GetSubscriber().UnsubscribeAll();
            Database.Multiplexer.Dispose();
            CacheClient.Dispose();
        }

        public bool Add<T>(string key, T value, DateTimeOffset expiresAt) where T : class
        {
            var serializedObject = JsonConvert.SerializeObject(value);
            var expiration = expiresAt.Subtract(DateTimeOffset.Now);
            return Database.StringSet(key, serializedObject, expiration);
        }

        public bool Add<T>(string key, T value) where T : class
        {
            var serializedObject = JsonConvert.SerializeObject(value);
            return Database.StringSet(key, serializedObject);
        }

        public T Get<T>(string key) where T : class
        {
            RedisValue serializedObject = Database.StringGet(key);
            if (!serializedObject.HasValue) return default(T);
            return JsonConvert.DeserializeObject<T>(serializedObject);
        }
    }
}