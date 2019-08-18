// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Core.Filters;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Exceptions.ValidationException;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Fluent.Architecture.Validation
{
    /// <summary>
    /// A classe de validação base de todas as validações com entidade do sistema.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FluentValidation<T> : TransactionalValidation where T : BaseEntity
    {
        #region INTERNAL

        /// <summary>
        /// A validação do serviço.
        /// </summary>
        protected internal new FluentService<T> Service
        {
            get => base.Service as FluentService<T>;
            set => base.Service = value;
        }

        public UserSessionRequest SessionRequest => Service.SessionRequest;

        // Todo2 documentar
        public bool NullParameterOk { get; set; } = true;

        // Todo2 documentar
        public bool KeyValuesOk { get; set; } = true;

        #endregion

        /// <summary>
        /// Validate add operation.
        /// </summary>
        /// <param name="entity">
        /// A entidade a ser validada.
        /// </param>
        public virtual void Add(T entity)
        {
            this.ParameterMustBeInformed(entity);
            this.RequiredPropertyMustBeInformed(entity);
            this.MaxLenghtPropertyMustBeInformed(entity);
            this.AllKeysShouldBeInformedWhenThereAreMoreThanOne(entity);

            if (KeyValuesOk)
            {
                EntityShouldNotExistInDatabaseBasedOnKeys(entity, false);
            }

            this.RunTheContextValidation();
        }

        public virtual void AddRange(T[] entities)
        {
            this.ParameterMustBeInformed(entities);

            if (entities != null)
            {
                foreach (var entity in entities)
                {
                    this.ParameterMustBeInformed(entity);
                    this.RequiredPropertyMustBeInformed(entity);
                    this.MaxLenghtPropertyMustBeInformed(entity);
                    this.AllKeysShouldBeInformedWhenThereAreMoreThanOne(entity);

                    if (KeyValuesOk)
                    {
                        EntityShouldNotExistInDatabaseBasedOnKeys(entity, false);
                    }
                }
            }

            this.RunTheContextValidation();
        }

        public virtual void Update(T entity)
        {
            this.ParameterMustBeInformed(entity);
            this.RequiredPropertyMustBeInformed(entity);
            this.MaxLenghtPropertyMustBeInformed(entity);
            this.AllKeysShouldBeInformedWhenThereAreMoreThanOne(entity, isUpdate: true);

            if (KeyValuesOk)
            {
                EntityMustExistInDatabase(entity);
            }

            this.RunTheContextValidation();
        }

        public virtual void UpdateRange(T[] entities)
        {
            this.ParameterMustBeInformed(entities);

            if (entities != null)
            {
                foreach (var entity in entities)
                {
                    this.ParameterMustBeInformed(entity);
                    this.RequiredPropertyMustBeInformed(entity);
                    this.MaxLenghtPropertyMustBeInformed(entity);
                    this.AllKeysShouldBeInformedWhenThereAreMoreThanOne(entity, isUpdate: true);
                    if (KeyValuesOk)
                    {
                        EntityMustExistInDatabase(entity);
                    }
                }
            }

            this.RunTheContextValidation();
        }

        public virtual void Remove(T entity)
        {
            this.ParameterMustBeInformed(entity);
            if (entity != null)
            {
                this.AllKeysMustBeInformed(entity);
                this.EntityMustExistInDatabase(entity);
            }

            this.RunTheContextValidation();
        }

        public virtual void RemoveRange(T[] entities)
        {
            this.ParameterMustBeInformed(entities);

            if (entities != null)
            {
                foreach (var entity in entities)
                {
                    this.ParameterMustBeInformed(entity);
                    //this.AllKeysMustBeInformed(entity);
                    this.EntityMustExistInDatabase(entity);
                }
            }

            this.RunTheContextValidation();
        }

        internal void FilteredList(Filter[] filters)
        {
            this.ParameterMustBeInformed(filters);

            var properties = typeof(T).GetProperties().ToList();

            foreach (var filter in filters)
            {
                var property = properties.SingleOrDefault(x => x.Name.Equals(filter.PropertyName, StringComparison.InvariantCultureIgnoreCase));
                if (property == null)
                {
                    AddInconsistency(new FilteredPropertyNotFound(typeof(T).Name, filter.PropertyName));
                }
            }

            this.RunTheContextValidation();
        }

        // Todo2 Documentar
        public virtual void Find(T entity, bool checkId = true)
        {
            ParameterMustBeInformed(entity);

            if (entity != null)
            {
                if (checkId)
                {
                    AllKeysMustBeInformed(entity);
                }
                else
                {
                    AllKeysShouldBeInformedWhenThereAreMoreThanOne(entity);
                }
            }

            this.RunTheContextValidation();
        }

        public virtual void FindByTerm(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                AddInconsistency(new NullParameterFluentValidationException(nameof(term)));
            }

            if (!typeof(T).GetProperties().Any(x => x.GetCustomAttribute<SearchableAttribute>() != null))
            {
                AddInconsistency(new EntityHasNotSearchableAttributeProperties(typeof(T).Name));
            }

            RunTheContextValidation();
        }

        /*
    === PADRÃO DE NOMECLATURA ===
    O que deve ser verdadeiro
    ParameterMustBeInformed 
    (O parâmetro deve ser informado. Se não for informado, teremos uma inconsistência)
    Evite escrever negação, mas quando não for possível evitar, escreva assim: EntityShouldNotExistInDatabase.
    A entida não pode existir. Se existir, teremos uma inconsistência.
    =============================         
         */
        #region VALIDATIONS

        private void ParameterMustBeInformed(object entity)
        {
            if (entity == null)
            {
                this.AddInconsistency(new NullParameterFluentValidationException(nameof(entity)));
                this.NullParameterOk = false;
                return;
            }

            this.NullParameterOk = true;
        }

        private void MaxLenghtPropertyMustBeInformed(T entity)
        {
            if (!this.NullParameterOk)
            {
                return;
            }

            var properties = typeof(T).GetPropertiesByAttribute<RequiredAttribute>();
            foreach (var property in properties)
            {
                var attr = property.GetCustomAttribute<MaxLengthAttribute>();
                if (attr == null)
                {
                    continue;
                }

                var value = property.GetValue(entity);
                if (!attr.IsValid(value))
                {
                    this.AddInconsistency(new UiFieldMaxLenghtFluentValidationException(property));
                }
            }
        }

        private void RequiredPropertyMustBeInformed(T entity)
        {
            if (!this.NullParameterOk)
            {
                return;
            }

            var properties = typeof(T).GetPropertiesByAttribute<RequiredAttribute>();
            foreach (var property in properties)
            {
                if (property.GetValue(entity).IsFluentNull())
                {
                    this.AddInconsistency(new UiFieldRequiredFluentValidationException(property));
                }
            }
        }

        private void AllKeysMustBeInformed(T entity)
        {
            this.KeyValuesOk = true;

            var properties = entity.GetType().GetKeyProperties();
            foreach (var property in properties)
            {
                if (property.GetValue(entity).IsFluentNull())
                {
                    this.AddInconsistency(new DbFieldRequiredFluentValidationException(property));
                    this.KeyValuesOk = false;
                }
            }
        }

        private void AllKeysShouldBeInformedWhenThereAreMoreThanOne(T entity, bool isUpdate = false)
        {
            if (!this.NullParameterOk || !this.KeyValuesOk)
            {
                return;
            }

            var entityType = typeof(T);
            var properties = entityType.GetKeyProperties();
            if (properties.Count > 1)
            {
                this.AllKeysMustBeInformed(entity);
            }
            else
            {
                var property = properties.First();
                if (property.GetValue(entity).IsFluentNull())
                {
                    if (!isUpdate)
                    {
                        return;
                    }

                    this.AddInconsistency(new DbFieldRequiredFluentValidationException(property));
                    this.KeyValuesOk = false;
                }
                else
                {
                    if (isUpdate)
                    {
                        return;
                    }

                    //Todo - Exigir que não seja informado somente quando o campo for de auto incremento.
                    //this.AddInconsistency(new DbFieldNotRequiredFluentValidationException(property));
                    //this.KeyValuesOk = false;
                }
            }
        }

        private void EntityMustExistInDatabase(T entity)
        {
            if (!this.NullParameterOk)
            {
                return;
            }

            if (!this.Service.Exists(entity, this.KeyValuesOk))
            {
                var keys = entity.GetKeyValues().Select(x => $"{{{x.Property.Name}:{x.Value}}}").ToArray();
                var keyValues = string.Join(", ", keys);
                this.AddInconsistency(new EntityNotFoundFluentValidationException(keyValues));
            }
        }

        private void EntityShouldNotExistInDatabaseBasedOnKeys(T entity, bool checkId)
        {
            if (!this.NullParameterOk || !this.KeyValuesOk)
            {
                return;
            }

            if (this.Service.Exists(entity, checkId))
            {
                var keys = entity.GetKeyAndFluentUniqueKeyValues().Select(x => $"{{{x.Property.Name}:{x.Value}}}").ToArray();
                var keyValues = string.Join(", ", keys);
                this.AddInconsistency(new EntityExistsFluentValidationException(keyValues));
            }
        }

        #endregion
    }
}
