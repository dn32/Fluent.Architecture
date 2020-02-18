
namespace dn32.infra.Redis
{
    public static class DnRedisExtension
    {
        public static Config UseRedis<Service>(this Config configClass, string redisConnectionString) where Service : DnRedisService
        {
            configClass.RedisConnectionString = redisConnectionString;
            configClass.RedisService = typeof(Service);
            return configClass;
        }
    }
}
