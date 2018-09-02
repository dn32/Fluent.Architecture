using Fluent.Architecture.Attributes;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Validation;

namespace Fluent.Architecture.Services
{
    public class FluentGlobalizedService<T> : FluentService<T> where T : FluentGlobalizedEntity
    {
        protected internal new FluentGlobalizedRepository<T> Repository => base.Repository as FluentGlobalizedRepository<T>;
        protected internal new FluentGlobalizedValidation<T> Validation => base.Validation as FluentGlobalizedValidation<T>;

        [Propagate]
        public virtual T Find(T entity, string language)
        {
            Validation.Find(entity, language);
            return Repository.Find(entity, language);
        }
    }
}