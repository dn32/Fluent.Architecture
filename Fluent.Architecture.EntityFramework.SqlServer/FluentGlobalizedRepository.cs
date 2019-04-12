//// -----------------------------------------------------------------------
//// <copyright company="Fluent System">
////     Copyright © Fluent System. All rights reserved.
////     TODOS OS DIREITOS RESERVADOS.
//// </copyright>
//// -----------------------------------------------------------------------

//// ReSharper disable CommentTypo

//#if NET461
//using System.Data.Entity;

//#else
//using Microsoft.EntityFrameworkCore;

//#endif

//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using Fluent.Architecture.Attributes;
//using Fluent.Architecture.Extensions;
//using Fluent.Architecture.Interfaces;
//using Fluent.Architecture.Model;
//using Fluent.Architecture.Sample.Test.SupportElements.Model;

//namespace Fluent.Architecture.EntityFramework.SqlServer
//{
//    /// <inheritdoc />
//    /// <summary>
//    /// Repositório base com entidade do sistema baseado em Entity Framework.
//    /// </summary>
//    /// <typeparam name="TE">
//    /// O tipo de entidade do repositório.
//    /// </typeparam>
//    public class FluentGlobalizedRepository<TE> : FluentSQLRepository<TE> where TE : FluentGlobalizedEntity
//    {
//        // Tradução ok
//        /// <summary>
//        /// Adiciona um item ao banco de dados.
//        /// </summary>
//        /// <param name="entity">
//        /// Item a ser adicionado.
//        /// </param>

//        public override TE Add(TE entity)
//        {
//            if (string.IsNullOrWhiteSpace(entity.Language))
//            {
//                entity.Language = FluentLanguage.DefaultLanguage;
//            }

//            entity.IsDefaultLanguage = true;

//            var entityAdded = base.Add(entity);

//            Service.AddInteractions(AddTranslation, entity);

//            return entityAdded;
//        }

//        // Tradução ok

//        public override void AddRange(params TE[] entities)
//        {
//            foreach (var entity in entities)
//            {
//                Add(entity);
//            }
//        }

//        // Tradução ok

//        public override TE Update(TE entity)
//        {
//            RunTheContextValidation();

//            var persistentEntity = Find(entity);

//            ((DbContext)TransactionObjects.Session).Entry(persistentEntity).CurrentValues.SetValues(entity);

//            if (!entity.IsDefaultLanguage)
//            {
//                DoNotAllowChangeGlobalizedProperties(persistentEntity);
//            }

//            var translations = ExtractTranslactionsOfEntity(entity);
//            var existentsTranslactions = FindTranslationsByLanguage(entity, entity.Language).ToList();
//            if (existentsTranslactions.Any())
//            {
//                existentsTranslactions.ForEach(x => x.Value = translations.FirstOrDefault(y => y.Property == x.Property)?.Value);
//            }
//            else
//            {
//                AddTranslation(entity);
//            }

//            return persistentEntity;
//        }

//        // Tradução ok

//        public virtual List<TE> List(IFluentSpecification spec, FluentPagination pagination, string language)
//        {
//            var list = base.List(spec, pagination);

//            list.ForEach(x => UpdateTranslationForFoundEntity(x, language));

//            return list;
//        }

//        // Tradução ok
//        //
//        //public virtual List<TE> List(string language)
//        //{
//        //    var list = base.List();

//        //    list.ForEach(x => UpdateTranslationForFoundEntity(x, language));

//        //    return list;
//        //}

//        // Tradução ok

//        public virtual TE FirstOrDefault(IFluentSpecification spec, string language)
//        {
//            var persistedEntity = base.FirstOrDefault(spec);
//            return UpdateTranslationForFoundEntity(persistedEntity, language);
//        }

//        // Tradução ok

//        public virtual TE FirstOrDefault(string language)
//        {
//            var persistedEntity = base.FirstOrDefault();
//            return UpdateTranslationForFoundEntity(persistedEntity, language);
//        }

//        // Tradução ok

//        public virtual TE Find(TE entity, string language)
//        {
//            var persistedEntity = base.Find(entity);
//            return UpdateTranslationForFoundEntity(persistedEntity, language);
//        }

//        // Tradução ok

//        public override TE Remove(TE entity)
//        {
//            TranslactionInput.RemoveRange(FindAllTranslationsOfAnEntity(entity));
//            return base.Remove(entity);
//        }

//        #region PRIVATE

//        private IQueryable<Translation> FindAllTranslationsOfAnEntity(TE entity)
//        {
//            var entityType = entity.GetTypeName();
//            var entityId = entity.GetKeyValue();

//            return TranslactionInput.Where(x => x.EntityType == entityType && x.EntityId == entityId);
//        }

//        private IQueryable<Translation> FindTranslationsByLanguage(TE entity, string language)
//        {
//            return FindAllTranslationsOfAnEntity(entity).Where(x => x.Language == language);
//        }

//        private void AddTranslation(FluentGlobalizedEntity entity)
//        {
//            var translations = ExtractTranslactionsOfEntity(entity);

//            foreach (var translation in translations)
//            {
//                TranslactionInput.Add(translation);
//            }
//        }

//        private static List<Translation> ExtractTranslactionsOfEntity(FluentGlobalizedEntity entity)
//        {
//            var properties = typeof(TE).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.GetCustomAttribute<FluentGlobalizationAttribute>() != null).ToList();
//            var translations = properties.Select(x =>
//                    new Translation
//                    {
//                        EntityType = entity.GetTypeName(),
//                        EntityId = entity.GetKeyValue(),
//                        Language = entity.Language,
//                        Property = x.Name,
//                        Value = x.GetValue(entity).ToString()
//                    })
//                .ToList();

//            return translations;
//        }

//        private TE UpdateTranslationForFoundEntity(TE persistedEntity, string language)
//        {
//            if (persistedEntity.Language == language)
//            {
//                return persistedEntity;
//            }

//            Session.Entry(persistedEntity).State = EntityState.Detached;

//            if (!UpdateTranslateOfEntity(persistedEntity, language))
//            {
//                return persistedEntity;
//            }

//            persistedEntity.Language = language;
//            persistedEntity.IsDefaultLanguage = false;

//            return persistedEntity;
//        }

//        private bool UpdateTranslateOfEntity(TE entity, string language)
//        {
//            var entityType = entity.GetType();
//            var translations = FindTranslationsByLanguage(entity, language).ToList();
//            translations.ForEach(translation => entityType.GetProperty(translation.Property)?.SetValue(entity, translation.Value));
//            return translations.Any();
//        }

//        private void DoNotAllowChangeGlobalizedProperties(TE persistedEntity)
//        {
//            var properties = typeof(TE).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.GetCustomAttribute<FluentGlobalizationAttribute>() != null).ToList();

//            properties.ForEach(property => Session.Entry(persistedEntity).Property(property.Name).IsModified = false);
//            Session.Entry(persistedEntity).Property(x => x.Language).IsModified = false;
//        }

//        #endregion
//    }
//}

