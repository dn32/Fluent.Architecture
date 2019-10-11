using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Services;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Fluent.Architecture.Redis
{
    public class FluentRedisService : TransactionalService
    {
        private FluentRedisRepository RedisRepository { get; set; }

        public FluentRedisService()
        {
            RedisRepository = new FluentRedisRepository(Setup.Config.Config.RedisConnectionString);
        }

        public async Task<T> GetValueAsync<T>(string key) => await RedisRepository.GetValueAsync<T>(key);

        public async Task<T> GetFluentEntityAsync<T>(FluentEntity entity) => await RedisRepository.GetValueAsync<T>(entity.GetHashCode().ToString());

        public async Task<bool> SetFluentEntityAsync(FluentEntity entity, TimeSpan? timeOut = null) => await RedisRepository.SetValueAsync(entity.GetHashCode().ToString(), entity, timeOut);

        public async Task<bool> SetValueAsync(string key, object value, TimeSpan? timeOut = null) => await RedisRepository.SetValueAsync(key, value, timeOut);

        public async Task<bool> SetPrimitiveValueAsync(string key, RedisValue value, TimeSpan? timeOut = null) => await RedisRepository.SetPrimitiveValueAsync(key, value, timeOut);

        public async Task<bool> RenewTimeOutAsync(string key, object stringValue = null) => await RedisRepository.RenewTimeOutAsync(key, stringValue);
    }
}
