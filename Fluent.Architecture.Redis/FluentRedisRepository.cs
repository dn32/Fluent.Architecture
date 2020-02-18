using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace dn32.infra.Redis
{
    internal class FluenteRedisRepository
    {
        private FluenteRedisContext Context { get; set; }

        internal FluenteRedisRepository(string connectionString)
        {
            Context = new FluenteRedisContext(connectionString);
        }

        internal async Task<T> GetValueAsync<T>(string key, bool renewTimeout = false)
        {
            return await Context.GetObjectAsync<T>($"{key}", renewTimeout);
        }

        internal async Task<bool> SetValueAsync(string key, object value, TimeSpan? timeOut = null)
        {
            return await Context.SetObjectAsync(key, value, timeOut);
        }

        internal async Task<bool> SetPrimitiveValueAsync(string key, RedisValue value, TimeSpan? timeOut = null)
        {
            return await Context.SetPrimitiveValueAsync(key, value, timeOut);
        }

        internal async Task<bool> RenewTimeOutAsync(string key, object stringValue = null)
        {
            return await Context.RenewTimeOut(key, stringValue);
        }
    }
}
