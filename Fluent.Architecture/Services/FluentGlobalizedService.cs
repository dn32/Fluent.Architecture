using System.Collections.Generic;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Interfaces;
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

        //[Propagate]
        //public virtual List<T> List(string language)
        //{
        //    Validation.LanguageValidate(language);
        //    return this.Repository.List(language);
        //}

        [Propagate]
        public virtual List<T> List(IFluentSpecification spec, FluentPagination pagination, string language)
        {
            Validation.LanguageValidate(language);
            return this.Repository.List(spec, pagination, language);
        }

        [Propagate]
        public virtual T Find(T entity, string language)
        {
            Validation.Find(entity, language);
            return Repository.Find(entity, language);
        }

        //Todo Doc
        [Propagate]
        public virtual T FirstOrDefault(string language)
        {
            Validation.FirstOrDefault(language);
            return this.Repository.FirstOrDefault(language);
        }

        [Propagate]
        public virtual T FirstOrDefault(IFluentSpecification spec, string language)
        {
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

            Validation.Add(entity);

            return base.Add(entity);
        }
    }
}