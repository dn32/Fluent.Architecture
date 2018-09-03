using System.Collections.Generic;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Validation;

namespace Fluent.Architecture.Services
{
    public class FluentGlobalizedService<T> : FluentService<T> where T : FluentGlobalizedEntity
    {
        protected internal new FluentGlobalizedRepository<T> Repository => base.Repository as FluentGlobalizedRepository<T>;
        protected internal new FluentGlobalizedValidation<T> Validation => base.Validation as FluentGlobalizedValidation<T>;

        [Propagate]
        public virtual List<T> List(FluentSpecification<T> spec, FluentPagination pagination, string language="")
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                language = Language.DefaultLanguage;
            }

            return this.Repository.List(spec, pagination, language);
        }

        [Propagate]
        public virtual T Find(T entity, string language = "")
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                language = Language.DefaultLanguage;
            }

            Validation.Find(entity, language);
            return Repository.Find(entity, language);
        }

        public virtual T FirstOrDefault(FluentSpecification<T> spec, string language)
        {

            if (string.IsNullOrWhiteSpace(language))
            {
                language = Language.DefaultLanguage;
            }

            Validation.FirstOrDefault(spec, language);
            return Repository.FirstOrDefault(spec, language);
        }

        [Propagate]
        public override T Add(T entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Language))
            {
                entity.Language = Language.DefaultLanguage;
            }

            return base.Add(entity);
        }
    }
}