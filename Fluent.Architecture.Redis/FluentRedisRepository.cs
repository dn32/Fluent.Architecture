using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace dn32.infra.Redis
{
    internal class DnRedisRepository
    {
        private DnRedisContext Context { get; set; }

        internal DnRedisRepository(string connectionString)
        {
            Context = new DnRedisContext(connectionString);
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
