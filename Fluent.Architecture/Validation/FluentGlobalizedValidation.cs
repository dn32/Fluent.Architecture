using System.Globalization;
using System.Linq;
using Fluent.Architecture.Exceptions.ValidationException;
using Fluent.Architecture.Model;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Validation
{
    public class FluentGlobalizedValidation<T> : FluentValidation<T> where T : FluentGlobalizedEntity
    {
        //Todo testar
        public virtual void Find(T entity, string language)
        {
            base.Find(entity);
            LanguageMustBeValid(language);
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
                AddInconsistency(new LanguageValidationException("The language should be informed"));
            }

            if (CultureInfo.GetCultures(CultureTypes.AllCultures).All(x => x.Name != language))
            {
                AddInconsistency(new LanguageValidationException($"{language} is an invalid language."));
            }
        }

        public void FirstOrDefault(FluentSpecification<T> spec, string language)
        {
            LanguageMustBeValid(language);
            RunTheContextValidation();
        }
    }
}