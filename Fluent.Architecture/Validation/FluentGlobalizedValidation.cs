using Fluent.Architecture.Model;

namespace Fluent.Architecture.Validation
{
    public class FluentGlobalizedValidation<T> : FluentValidation<T> where T : FluentGlobalizedEntity
    {
        //Todo - validate language
        public virtual void Find(T entity, string language)
        {
            base.Find(entity);
        }
    }
}