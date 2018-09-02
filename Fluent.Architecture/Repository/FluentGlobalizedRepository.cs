// ReSharper disable CommentTypo
#if NET461

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Model;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Repository
{
    /// <inheritdoc />
    /// <summary>
    /// Repositório base com entidade do sistema baseado em Entity Framework.
    /// </summary>
    /// <typeparam name="TE">
    /// O tipo de entidade do repositório.
    /// </typeparam>
    public class FluentGlobalizedRepository<TE> : FluentRepository<TE> where TE : FluentGlobalizedEntity
    {
        public void AddLanguageData(FluentGlobalizedEntity entity)
        {
            var language = Language.Get(entity.Language);

            var properties = typeof(TE).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.GetCustomAttribute<FluentGlobalizationAttribute>() != null).ToList();
            var translations = properties.Select(x =>
                new Translation
                {
                    EntityType = entity.GetTableName(),
                    EntityId = entity.GetKeyValue(),
                    LanguageId = language.Id,
                    Property = x.GetColumnName(),
                    Value = x.GetValue(entity).ToString()
                })
                .ToList();

            foreach (var translation in translations)
            {
                TranslactionInput.Add(translation);
            }
        }

        /// <summary>
        /// Adiciona um item ao banco de dados.
        /// </summary>
        /// <param name="entity">
        /// Item a ser adicionado.
        /// </param>
        [Propagate]
        public override TE Add(TE entity)
        {
            RunTheContextValidation();

            entity.IsDefaultLanguage = true;

            Service.AddInteractions(AddLanguageData, entity);

            return base.Add(entity);
        }
    }
}
#endif
