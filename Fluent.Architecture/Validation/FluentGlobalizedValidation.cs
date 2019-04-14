// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using System.Linq;
using Fluent.Architecture.Exceptions.ValidationException;
using Fluent.Architecture.Interfaces;
using Fluent.Architecture.Entities;

namespace Fluent.Architecture.Validation
{
    public class FluentGlobalizedValidation<T> : FluentValidation<T> where T : FluentGlobalizedEntity
    {
        //Todo testar
        public virtual void Find(T entity, string language)
        {
            LanguageMustBeValid(language);
            base.Find(entity);
        }

        //Todo testar
        public override void Add(T entity)
        {
            base.Add(entity);
            LanguageMustBeValid(entity.Language);
        }

        private void LanguageMustBeValid(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                AddInconsistency(new LanguageValidationException("The language should be informed."));
            }

            if (CultureInfo.GetCultures(CultureTypes.AllCultures).All(x => x.Name != language))
            {
                AddInconsistency(new LanguageValidationException($"{language} is an invalid language."));
            }
        }

        public void FirstOrDefault(string language)
        {
            LanguageValidate(language);
        }

        public void LanguageValidate(string language)
        {
            LanguageMustBeValid(language);
            RunTheContextValidation();
        }
    }
}