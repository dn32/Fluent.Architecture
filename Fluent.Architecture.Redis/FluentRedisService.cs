using Fluente.Arquitetura.Nucleo.Models;
using Fluente.Arquitetura.Services;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Fluente.Arquitetura.Redis
{
    public class FluenteRedisService : TransactionalService
    {
        private FluenteRedisRepository RedisRepository { get; set; }

        public FluenteRedisService()
        {
            RedisRepository = new FluenteRedisRepository(Setup.Config.Config.RedisConnectionString);
        }

        public async Task<T> GetValueAsync<T>(string key) => await RedisRepository.GetValueAsync<T>(key);

        public async Task<T> GetFluenteEntityAsync<T>(FluenteEntidade entity) => await RedisRepository.GetValueAsync<T>(entity.GetHashCode().ToString());

        public async Task<bool> SetFluenteEntityAsync(FluenteEntidade entity, TimeSpan? timeOut = null) => await RedisRepository.SetValueAsync(entity.GetHashCode().ToString(), entity, timeOut);

        public async Task<bool> SetValueAsync(string key, object value, TimeSpan? timeOut = null) => await RedisRepository.SetValueAsync(key, value, timeOut);

        public async Task<bool> SetPrimitiveValueAsync(string key, RedisValue value, TimeSpan? timeOut = null) => await RedisRepository.SetPrimitiveValueAsync(key, value, timeOut);

        public async Task<bool> RenewTimeOutAsync(string key, object stringValue = null) => await RedisRepository.RenewTimeOutAsync(key, stringValue);
    }
}
