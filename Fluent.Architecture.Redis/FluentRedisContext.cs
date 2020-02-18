using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using dn32.infra.extensoes;

namespace dn32.infra.Redis
{
    public class DnRedisContext
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


        public DnRedisContext(string connectionString)
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
            return await Db.StringSetAsync($"{key}:valor", JsonConvert.SerializeObject(value), expireTime);
        }

        public async Task<bool> SetPrimitiveValueAsync(string key, RedisValue redisValue, TimeSpan? expireTime = null)
        {
            return await Db.StringSetAsync($"{key}:valor", redisValue, expireTime);
        }

        public async Task<T> GetPrimitiveAsync<T>(string key, bool renewTimeout = false)
        {
            var stringValue = await Db.StringGetAsync($"{key}:valor");
            if (string.IsNullOrEmpty(stringValue)) return default;
            if (renewTimeout) { await RenewTimeOut(key, stringValue); }
            var newValue = Convert.ChangeType(stringValue, typeof(T));
            return newValue.DnCast<T>();
        }

        public async Task<T> GetObjectAsync<T>(string key, bool renewTimeout = false)
        {
            var stringValue = await Db.StringGetAsync($"{key}:valor");
            if (string.IsNullOrEmpty(stringValue)) { return default; }
            if (renewTimeout) { await RenewTimeOut(key, stringValue); }
            return JsonConvert.DeserializeObject<T>(stringValue);
        }

        public async Task<bool> RenewTimeOut(string key, object stringValue = null)
        {
            var redisValueTime = await Db.StringGetAsync($"{key}:time");
            stringValue ??= await Db.StringGetAsync($"{key}:valor");
            var time = redisValueTime.ToString();
            if (!string.IsNullOrEmpty(time))
            {
                var redisValue = stringValue.DnCast<RedisValue>();
                return await Db.StringSetAsync($"{key}:valor", redisValue, TimeSpan.FromMinutes(Convert.ToDouble(time)));
            }

            return false;
        }
    }
}
