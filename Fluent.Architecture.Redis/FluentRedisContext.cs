using System;
using System.Threading.Tasks;
using Fluent.Architecture.Extensions;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Fluent.Architecture.Redis
{
    public class FluentRedisContext
    {
        private ConnectionMultiplexer _multiplexer { get; set; }

        private ConfigurationOptions ConfigurationOptions { get; set; }

        private IDatabase Db => Multiplexer.GetDatabase();

        public ConnectionMultiplexer Multiplexer
        {
            get
            {
                if (_multiplexer == null || !_multiplexer.IsConnected)
                {
                    _multiplexer = ConnectionMultiplexer.Connect(ConfigurationOptions);
                }

                return _multiplexer;
            }
        }


        public FluentRedisContext(string connectionString)
        {
            ConfigurationOptions = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                SyncTimeout = int.MaxValue,
                ConnectTimeout = 3000,
                EndPoints = { connectionString }
            };
        }

        public async Task<bool> SetObjectAsync(string key, object value, TimeSpan? expireTime = null)
        {
            return await Db.StringSetAsync($"{key}:value", JsonConvert.SerializeObject(value), expireTime);
        }

        public async Task<bool> SetPrimitiveValueAsync(string key, RedisValue redisValue, TimeSpan? expireTime = null)
        {
            return await Db.StringSetAsync($"{key}:value", redisValue, expireTime);
        }

        public async Task<T> GetPrimitiveAsync<T>(string key, bool renewTimeout = false)
        {
            var stringValue = await Db.StringGetAsync($"{key}:value");
            if (string.IsNullOrEmpty(stringValue)) return default;
            if (renewTimeout) { await RenewTimeOut(key, stringValue); }
            var newValue = Convert.ChangeType(stringValue, typeof(T));
            return newValue.FluentCast<T>();
        }

        public async Task<T> GetObjectAsync<T>(string key, bool renewTimeout = false)
        {
            var stringValue = await Db.StringGetAsync($"{key}:value");
            if (string.IsNullOrEmpty(stringValue)) { return default; }
            if (renewTimeout) { await RenewTimeOut(key, stringValue); }
            return JsonConvert.DeserializeObject<T>(stringValue);
        }

        public async Task<bool> RenewTimeOut(string key, object stringValue = null)
        {
            var redisValueTime = await Db.StringGetAsync($"{key}:time");
            stringValue ??= await Db.StringGetAsync($"{key}:value");
            var time = redisValueTime.ToString();
            if (!string.IsNullOrEmpty(time))
            {
                var redisValue = stringValue.FluentCast<RedisValue>();
                return await Db.StringSetAsync($"{key}:value", redisValue, TimeSpan.FromMinutes(Convert.ToDouble(time)));
            }

            return false;
        }
    }
}
