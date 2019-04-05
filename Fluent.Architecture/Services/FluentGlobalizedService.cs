// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

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

        //
        //public virtual List<T> List(string language)
        //{
        //    Validation.LanguageValidate(language);
        //    return this.Repository.List(language);
        //}


        public virtual List<T> List(IFluentSpecification spec, FluentPagination pagination, string language)
        {
            Validation.LanguageValidate(language);
            return Repository.List(spec, pagination, language);
        }

        public virtual List<T> List(IFluentSpecification spec, string language)
        {
            Validation.LanguageValidate(language);
            return Repository.List(spec, null, language);
        }

        public virtual List<T> List(string language)
        {
            Validation.LanguageValidate(language);
            var spec = CreateSpec<AllSpec<T>>();
            return Repository.List(spec, null, language);
        }


        public virtual T Find(T entity, string language)
        {
            Validation.Find(entity, language);
            return Repository.Find(entity, language);
        }

        //Todo Doc
        
        public virtual T FirstOrDefault(string language)
        {
            Validation.FirstOrDefault(language);
            return this.Repository.FirstOrDefault(language);
        }

        
        public virtual T FirstOrDefault(IFluentSpecification spec, string language)
        {
            Validation.FirstOrDefault(language);
            return Repository.FirstOrDefault(spec, language);
        }

        
        public override T Add(T entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Language))
            {
                entity.Language = FluentLanguage.DefaultLanguage;
            }

            Validation.Add(entity);

            return base.Add(entity);
        }
    }
}