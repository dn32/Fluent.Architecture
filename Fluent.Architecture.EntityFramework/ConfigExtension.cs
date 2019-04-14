namespace Fluent.Architecture.EntityFramework
{
    public static class ConfigExtension
    {
        public static Config UseEntityFramework(this Config configClass)
        {
            return configClass.SetRepositoryFactory(new RepositoryFactory());
        }
    }
}
