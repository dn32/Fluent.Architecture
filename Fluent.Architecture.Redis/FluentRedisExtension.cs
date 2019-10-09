
namespace Fluent.Architecture.Redis
{
    public static class FluentRedisExtension
    {
        public static Config UseRedis<Service>(this Config configClass, string redisConnectionString) where Service : FluentRedisService
        {
            configClass.RedisConnectionString = redisConnectionString;
            configClass.RedisService = typeof(Service);
            return configClass;
        }
    }
}
