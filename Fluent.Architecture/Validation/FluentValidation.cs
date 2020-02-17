// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluente.Arquitetura.Nucleo.Atributos;
using Fluente.Arquitetura.Nucleo.Enumerator;
using Fluente.Arquitetura.Nucleo.Models;
using Fluente.Arquitetura.Exceptions.ValidationException;
using Fluente.Arquitetura.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Fluente.Arquitetura.Validation
{
    /// <summary>
    /// A classe de validação base de todas as validações com entidade do sistema.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FluenteValidation<T> : TransactionalValidation, IFluenteValidation where T : EntidadeBase
    {
        #region INTERNAL

        /// <summary>
        /// A validação do serviço.
        /// </summary>
        protected internal new FluenteService<T> Service
        {
            get => base.Service as FluenteService<T>;
            set => base.Service = value;
        }

        public UserSessionRequest SessionRequest => Service.SessionRequest;

        // Todo2 documentar
        public bool NullParameterOk { get; set; } = true;

        // Todo2 documentar
        public bool KeyValuesOk { get; set; } = true;

        #endregion

        #region VALIDATE COMPOSITIONS

        // Composition
        public virtual async Task AddAsync(T entity)
        {
            var method = typeof(FluenteValidation<T>).GetMethod(nameof(AdddAsyncInternal), BindingFlags.NonPublic | BindingFlags.Static);

            var anotherServices = await (this).ExecuteEntityAndCompositions(entity, method);
            RunTheContextValidation(anotherServices);
        }

        // Composition
        public virtual async Task<List<TransactionalService>> AddOrUpdateAsync(T entity)
        {
            var method = typeof(FluenteValidation<T>).GetMethod(nameof(AdddAsyncInternal), BindingFlags.NonPublic | BindingFlags.Static);
            return await (this).ExecuteEntityAndCompositions(entity, method);
        }

        // Composition
        public virtual async Task UpdateAsync(T entity)
        {
            var method = typeof(FluenteValidation<T>).GetMethod(nameof(UpdateRangeAsyncInternal), BindingFlags.NonPublic | BindingFlags.Static);
            var anotherServices = await (this).ExecuteEntityAndCompositions(entity, method);

            if (KeyValuesOk)
            {
                await this.EntityMustExistInDatabaseAsync(entity, true);
                await this.ThereIsOnlyOneEntityAsync(entity, false);
            }

            RunTheContextValidation(anotherServices);
        }

        // Composition
        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            this.ParameterMustBeInformed(entities, nameof(entities));
            var anotherServices = new List<TransactionalService>();
            var method = typeof(FluenteValidation<T>).GetMethod(nameof(UpdateRangeAsyncInternal), BindingFlags.NonPublic | BindingFlags.Static);

            if (entities != null)
            {
                foreach (var entity_ in entities)
                {
                    var services = await (this).ExecuteEntityAndCompositions(entity_, method);
                    anotherServices.AddRange(services);

                    if (KeyValuesOk)
                    {
                        await this.EntityMustExistInDatabaseAsync(entity_);
                        //Todo validate ThereIsOnlyOneEntity(entity, false);
                        //Todo validate logical delete
                    }
                }
            }

            RunTheContextValidation(anotherServices);
        }

        // Composition
        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            this.ParameterMustBeInformed(entities, nameof(entities));
            var anotherServices = new List<TransactionalService>();
            if (entities != null)
            {
                var method = typeof(FluenteValidation<T>).GetMethod(nameof(AdddAsyncInternal), BindingFlags.NonPublic | BindingFlags.Static);

                foreach (var entity in entities)
                {
                    var services = await (this).ExecuteEntityAndCompositions(entity, method);
                    anotherServices.AddRange(services);
                }
            }

            RunTheContextValidation(anotherServices);
        }

        #endregion

        public virtual async Task RemoveAsync(T entity)
        {
            this.ParameterMustBeInformed(entity, null);
            if (entity != null)
            {
                this.AllKeysMustBeInformed(entity, null, null);
                await this.EntityMustExistInDatabaseAsync<T>(entity);
            }

            RunTheContextValidation();
        }

        public virtual async Task RemoveRangeAsync(T[] entities)
        {
            this.ParameterMustBeInformed(entities, null);

            if (entities != null)
            {
                foreach (var entity in entities)
                {
                    this.ParameterMustBeInformed(entity, null);
                    await this.EntityMustExistInDatabaseAsync(entity);
                }
            }

            RunTheContextValidation();
        }

        internal void FilteredList(Filtro[] filters)
        {
            this.ParameterMustBeInformed(filters, nameof(filters));

            var properties = typeof(T).GetProperties().ToList();

            foreach (var filter in filters)
            {
                var property = properties.SingleOrDefault(x => x.Name.Equals(filter.NomeDaPropriedade, StringComparison.InvariantCultureIgnoreCase));
                if (property == null)
                {
                    AddInconsistency(new FilteredPropertyNotFound(typeof(T).Name, filter.NomeDaPropriedade));
                }
            }

            RunTheContextValidation();
        }

        public virtual void Find(T entity, bool checkId = true)
        {
            this.ParameterMustBeInformed(entity, null);

            if (checkId && entity != null)
            {
                this.AllKeysMustBeInformed(entity, null, null);
            }

            RunTheContextValidation();
        }

        public virtual void FindByTerm(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                AddInconsistency(new NullParameterFluenteValidationException(nameof(term)));
            }

            if (!typeof(T).GetProperties().Any(x => x.GetCustomAttribute<SearchableAttribute>() != null))
            {
                AddInconsistency(new EntityHasNotSearchableAttributeProperties(typeof(T).Name));
            }

            RunTheContextValidation();
        }

        public void Truncate(string ERASE_ALL_DATA)
        {
            if (ERASE_ALL_DATA?.Equals("Yes", StringComparison.InvariantCultureIgnoreCase) != true)
            {
                AddInconsistency(new AlterLossOfDadaValidationException());
            }

            RunTheContextValidation();
        }

        #region INTERNAL

        internal static void UpdateAsyncInternal<T2>(IFluenteValidation validation, T2 entity, string compositionProperty, string compositionFieldName) where T2 : EntidadeBase
        {
            validation.ParameterMustBeInformed(entity, compositionProperty);
            validation.FluenteValidateAttribute(entity, compositionProperty, compositionFieldName);
            validation.RequiredPropertyMustBeInformed(entity, compositionProperty, compositionFieldName);
            validation.MaxMinLenghtPropertyMustBeInformed(entity, compositionProperty, compositionFieldName);
            validation.AllKeysShouldBeInformedWhenThereAreMoreThanOne(entity, compositionProperty, compositionFieldName);
        }

        internal static async Task AdddAsyncInternal<T2>(IFluenteValidation validation, T2 entity, string compositionProperty, string compositionFieldName) where T2 : EntidadeBase
        {
            validation.ParameterMustBeInformed(entity, compositionProperty);
            validation.FluenteValidateAttribute(entity, compositionProperty, compositionFieldName);
            validation.RequiredPropertyMustBeInformed(entity, compositionProperty, compositionFieldName);
            validation.MaxMinLenghtPropertyMustBeInformed(entity, compositionProperty, compositionFieldName);
            validation.AllKeysShouldBeInformedWhenThereAreMoreThanOne(entity, compositionProperty, compositionFieldName);

            if (validation.KeyValuesOk)
            {
                await validation.EntityShouldNotExistInDatabaseBasedOnKeysAsync(entity, false);
            }
        }

        internal static void UpdateRangeAsyncInternal<T2>(IFluenteValidation validation, T2 entity, string compositionProperty, string compositionFieldName) where T2 : EntidadeBase
        {
            validation.ParameterMustBeInformed(entity, compositionProperty);
            validation.FluenteValidateAttribute(entity, compositionProperty, compositionFieldName);
            validation.RequiredPropertyMustBeInformed(entity, compositionProperty, compositionFieldName);
            validation.MaxMinLenghtPropertyMustBeInformed(entity, compositionProperty, compositionFieldName);
            validation.AllKeysShouldBeInformedWhenThereAreMoreThanOne(entity, compositionProperty, compositionFieldName, isUpdate: true);
        }

        #endregion
    }
}
