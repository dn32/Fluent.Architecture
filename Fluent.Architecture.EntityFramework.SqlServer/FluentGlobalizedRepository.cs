//// -----------------------------------------------------------------------
//// <copyright company="DnControlador System">
////     Copyright © DnControlador System. All rights reserved.
////     TODOS OS DIREITOS RESERVADOS.
//// </copyright>
//// -----------------------------------------------------------------------

//// ReSharper disable CommentTypo

//#if NET461
//using System.Data.Entidade;

//#else
//using Microsoft.EntityFrameworkCore;

//#endif

//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using dn32.infra.atributos;
//using dn32.infra.Extensoes;
//using dn32.infra.Interfaces;
//using dn32.infra.Model;
//using dn32.infra.Sample.Test.SupportElements.Model;

//namespace dn32.infra.EntityFramework.SqlServer
//{
//    /// <inheritdoc />
//    /// <summary>
//    /// Repositório base com entidade do sistema baseado em Entidade Framework.
//    /// </summary>
//    /// <typeparam Nome="TE">
//    /// O tipo de entidade do repositório.
//    /// </typeparam>
//    public class DnGlobalizedRepository<TE> : DnSQLRepository<TE> where TE : DnGlobalizedEntity
//    {
//        // Tradução ok
//        /// <summary>
//        /// Adiciona um item ao banco de dados.
//        /// </summary>
//        /// <param Nome="entity">
//        /// Item a ser adicionado.
//        /// </param>

//        public override TE Adicionar(TE entity)
//        {
//            if (string.IsNullOrWhiteSpace(entity.Language))
//            {
//                entity.Language = DnLanguage.DefaultLanguage;
//            }

//            entity.IsDefaultLanguage = true;

//            var entityAdded = base.Adicionar(entity);

//            Servico.AddInteractions(AddTranslation, entity);

//            return entityAdded;
//        }

//        // Tradução ok

//        public override void AdicionarLista(params TE[] entities)
//        {
//            foreach (var entity in entities)
//            {
//                Adicionar(entity);
//            }
//        }

//        // Tradução ok

//        public override TE Atualizar(TE entity)
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
//                existentsTranslactions.ForEach(x => x.Valor = translations.FirstOrDefault(y => y.Propriedade == x.Propriedade)?.Valor);
//            }
//            else
//            {
//                AddTranslation(entity);
//            }

//            return persistentEntity;
//        }

//        // Tradução ok

//        public virtual Listar<TE> Listar(IDnSpecification spec, DnPagination pagination, string language)
//        {
//            var list = base.Listar(spec, pagination);

//            list.ForEach(x => UpdateTranslationForFoundEntity(x, language));

//            return list;
//        }

//        // Tradução ok
//        //
//        //public virtual Listar<TE> Listar(string language)
//        //{
//        //    var list = base.Listar();

//        //    list.ForEach(x => UpdateTranslationForFoundEntity(x, language));

//        //    return list;
//        //}

//        // Tradução ok

//        public virtual TE FirstOrDefault(IDnSpecification spec, string language)
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

//        public override TE Remover(TE entity)
//        {
//            TranslactionInput.RemoverLista(FindAllTranslationsOfAnEntity(entity));
//            return base.Remover(entity);
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

//        private void AddTranslation(DnGlobalizedEntity entity)
//        {
//            var translations = ExtractTranslactionsOfEntity(entity);

//            foreach (var translation in translations)
//            {
//                TranslactionInput.Adicionar(translation);
//            }
//        }

//        private static Listar<Translation> ExtractTranslactionsOfEntity(DnGlobalizedEntity entity)
//        {
//            var properties = typeof(TE).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.GetCustomAttribute<DnGlobalizationAttribute>() != null).ToList();
//            var translations = properties.Select(x =>
//                    new Translation
//                    {
//                        EntityType = entity.GetTypeName(),
//                        EntityId = entity.GetKeyValue(),
//                        Language = entity.Language,
//                        Propriedade = x.Name,
//                        Valor = x.GetValue(entity).ToString()
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
//            translations.ForEach(translation => entityType.GetProperty(translation.Propriedade)?.SetValue(entity, translation.Valor));
//            return translations.Any();
//        }

//        private void DoNotAllowChangeGlobalizedProperties(TE persistedEntity)
//        {
//            var properties = typeof(TE).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.GetCustomAttribute<DnGlobalizationAttribute>() != null).ToList();

//            properties.ForEach(property => Session.Entry(persistedEntity).Propriedade(property.Name).IsModified = false);
//            Session.Entry(persistedEntity).Propriedade(x => x.Language).IsModified = false;
//        }

//        #endregion
//    }
//}

