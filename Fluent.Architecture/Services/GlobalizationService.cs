
namespace Fluent.Architecture.Services
{
    public abstract class GlobalizationService : TransactionalService
    {
        public abstract string GetResource(string key, string defaultMessage, params string[] parameters);
    }
}
