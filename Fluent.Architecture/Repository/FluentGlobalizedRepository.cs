// ReSharper disable CommentTypo
#if NET461

using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Model;
using Fluent.Architecture.Sample.Test.SupportElements.Model;

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
        private void AddTranslation(FluentGlobalizedEntity entity)
        {
            var translations = ExtractTranslactionsOfEntity(entity);

            foreach (var translation in translations)
            {
                TranslactionInput.Add(translation);
            }
        }

        private static List<Translation> ExtractTranslactionsOfEntity(FluentGlobalizedEntity entity)
        {
            var properties = typeof(TE).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.GetCustomAttribute<FluentGlobalizationAttribute>() != null).ToList();
            var translations = properties.Select(x =>
                    new Translation
                    {
                        EntityType = entity.GetTypeName(),
                        EntityId = entity.GetKeyValue(),
                        Language = entity.Language,
                        Property = x.Name,
                        Value = x.GetValue(entity).ToString()
                    })
                .ToList();

            return translations;
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
            if (string.IsNullOrWhiteSpace(entity.Language))
            {
                entity.Language = Language.DefaultLanguage;
            }

            entity.IsDefaultLanguage = true;

            var entityAdded = base.Add(entity);

            Service.AddInteractions(AddTranslation, entity);

            return entityAdded;
        }

        private IQueryable<Translation> FindAllTranslationsOfAnEntity(TE entity)
        {
            if (entity.GetKeyValue() == 0)
            {
                throw new InvalidExpressionException();
            }

            var entityType = entity.GetTypeName();
            var entityId = entity.GetKeyValue();

            return TranslactionInput.Where(x => x.EntityType == entityType && x.EntityId == entityId);
        }

        private IQueryable<Translation> FindTranslationsByLanguage(TE entity, string language)
        {
            return FindAllTranslationsOfAnEntity(entity).Where(x => x.Language == language);
        }

        [Propagate]
        public override TE Update(TE entity)
        {
            RunTheContextValidation();

            var persistentEntity = Find(entity);

            TransactionObjects.Session.Entry(persistentEntity).CurrentValues.SetValues(entity);

            if (!entity.IsDefaultLanguage)
            {
                DoNotAllowChangeGlobalizedProperties(entity, persistentEntity);
            }

            var translations = ExtractTranslactionsOfEntity(entity);
            var existentsTranslactions = FindTranslationsByLanguage(entity, entity.Language).ToList();
            if (existentsTranslactions.Any())
            {
                existentsTranslactions.ForEach(x => x.Value = translations.FirstOrDefault(y => y.Property == x.Property)?.Value);
            }
            else
            {
                AddTranslation(entity);
            }

            return persistentEntity;
        }

        [Propagate]
        public virtual TE Find(TE entity, string language)
        {
            var persistedEntity = base.Find(entity);
            Session.Entry(persistedEntity).State = EntityState.Detached;

            if (persistedEntity.Language != language)
            {
                UpdateTranslateOfEntity(persistedEntity, language);
            }

            return persistedEntity;
        }

        private void UpdateTranslateOfEntity(TE entity, string language)
        {
            var entityType = entity.GetType();
            var translations = FindTranslationsByLanguage(entity, language).ToList();
            translations.ForEach(translation => entityType.GetProperty(translation.Property)?.SetValue(entity, translation.Value));
        }

        [Propagate]
        public override TE Remove(TE entity)
        {
            TranslactionInput.RemoveRange(FindAllTranslationsOfAnEntity(entity));
            return base.Remove(entity);
        }

        private void DoNotAllowChangeGlobalizedProperties(TE entity, TE persistedEntity)
        {
            var properties = typeof(TE).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.GetCustomAttribute<FluentGlobalizationAttribute>() != null).ToList();

            properties.ForEach(property => Session.Entry(persistedEntity).Property(property.Name).IsModified = false);
            Session.Entry(persistedEntity).Property(x => x.Language).IsModified = false;
        }
    }
}
#endif
