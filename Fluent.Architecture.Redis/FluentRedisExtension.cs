
namespace dn32.infra.Redis
{
    public static class FluenteRedisExtension
    {
        public static Config UseRedis<Service>(this Config configClass, string redisConnectionString) where Service : FluenteRedisService
        {
            configClass.RedisConnectionString = redisConnectionString;
            configClass.RedisService = typeof(Service);
            return configClass;
        }
    }
}
