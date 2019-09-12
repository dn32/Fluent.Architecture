// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Core.Controllers.ControllerModel;
using Fluent.Architecture.Core.Filters;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Exceptions.ValidationException;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
            this.MaxMinLenghtPropertyMustBeInformed(entity);
            this.AllKeysShouldBeInformedWhenThereAreMoreThanOne(entity);

            if (KeyValuesOk)
            {
                EntityShouldNotExistInDatabaseBasedOnKeys(entity, false);
            }

            this.RunTheContextValidation();
        }

        public virtual void AddOrUpdate(T entity)
        {
            this.ParameterMustBeInformed(entity);
            this.RequiredPropertyMustBeInformed(entity);
            this.MaxMinLenghtPropertyMustBeInformed(entity);
            this.AllKeysShouldBeInformedWhenThereAreMoreThanOne(entity);

            if (KeyValuesOk)
            {
                EntityShouldNotExistInDatabaseBasedOnKeys(entity, false);
            }
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
                    this.MaxMinLenghtPropertyMustBeInformed(entity);
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
            this.MaxMinLenghtPropertyMustBeInformed(entity);
            this.AllKeysShouldBeInformedWhenThereAreMoreThanOne(entity, isUpdate: true);

            if (KeyValuesOk)
            {
                EntityMustExistInDatabase(entity, true);
                ThereIsOnlyOneEntity(entity, false);
            }

            this.RunTheContextValidation();
        }

        internal void UpdateAlter(UpdateAlter<T> value)
        {
            ParameterMustBeInformed(value);
            ParameterMustBeInformed(value.Original);
            ParameterMustBeInformed(value.Final);
            RequiredPropertyMustBeInformed(value.Original);
            RequiredPropertyMustBeInformed(value.Final);
            MaxMinLenghtPropertyMustBeInformed(value.Final);
            AllKeysShouldBeInformedWhenThereAreMoreThanOne(value.Final, isUpdate: true);

            if (KeyValuesOk)
            {
                EntityMustExistInDatabase(value.Original);
                //Todo validate ThereIsOnlyOneEntity(entity, false);
                //Todo validate logical delete
            }

            RunTheContextValidation();
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
                    this.MaxMinLenghtPropertyMustBeInformed(entity);
                    this.AllKeysShouldBeInformedWhenThereAreMoreThanOne(entity, isUpdate: true);
                    if (KeyValuesOk)
                    {
                        EntityMustExistInDatabase(entity);
                        //Todo validate ThereIsOnlyOneEntity(entity, false);
                        //Todo validate logical delete
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

            if (checkId && entity != null)
            {
                AllKeysMustBeInformed(entity);
            }

            RunTheContextValidation();
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

        public void Truncate(string ERASE_ALL_DATA)
        {
            if (ERASE_ALL_DATA?.Equals("Yes", StringComparison.InvariantCultureIgnoreCase) != true)
            {
                AddInconsistency(new AlterLossOfDadaValidationException());
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

        private void MaxMinLenghtPropertyMustBeInformed(T entity)
        {
            if (!this.NullParameterOk)
            {
                return;
            }

            var properties = entity.GetType().GetProperties().ToList();
            foreach (var property in properties)
            {
                var value = property.GetValue(entity);
                if (property.PropertyType.IsNumeric())
                {
                    var min = property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.min ?? property.GetCustomAttribute<RangeAttribute>()?.Minimum;
                    var max = property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.max ?? property.GetCustomAttribute<RangeAttribute>()?.Maximum;
                    if (min == null || max == null) { continue; }
                    var mindouble = double.Parse(min.ToString(), CultureInfo.InvariantCulture);
                    var maxdouble = double.Parse(max.ToString(), CultureInfo.InvariantCulture);
                    var valuedoble = double.Parse(value.ToString(), CultureInfo.InvariantCulture);
                    if (!new RangeAttribute(mindouble, maxdouble).IsValid(valuedoble))
                    {
                        AddInconsistency(new UiFieldLenghtFluentValidationException(property));
                    }
                }

                if (property.PropertyType == typeof(string) && property.PropertyType == typeof(String))
                {
                    var requ = property.GetCustomAttribute<RequiredAttribute>() != null || property.GetCustomAttribute<FluentRequiredAttribute>() != null;
                    if (requ && property.GetValue(entity).IsFluentNull())
                    {//Nesse caso já há uma inconsistência de requerido adicionada
                        return;
                    }

                    var min = property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.min ?? property.GetCustomAttribute<MinLengthAttribute>()?.Length;
                    var max = property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.max ?? property.GetCustomAttribute<MaxLengthAttribute>()?.Length;
                    if (min == null || max == null) { continue; }

                    if (!new MinLengthAttribute(min.Value).IsValid(value))
                    {
                        this.AddInconsistency(new UiFieldLenghtFluentValidationException(property));
                    }

                    var maxint = Convert.ChangeType(max, typeof(int), CultureInfo.InvariantCulture) as int?;
                    if (!new MaxLengthAttribute(maxint.Value).IsValid(value))
                    {
                        this.AddInconsistency(new UiFieldLenghtFluentValidationException(property));
                    }
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
            var properties2 = typeof(T).GetPropertiesByAttribute<FluentRequiredAttribute>();

            properties2.ForEach(x =>
            {
                if (!properties.Contains(x))
                {
                    properties.Add(x);
                }
            });

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
                    this.AddInconsistency(new UiFieldRequiredFluentValidationException(property));
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

                    this.AddInconsistency(new UiFieldRequiredFluentValidationException(property));
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

        private void EntityMustExistInDatabase(T entity, bool includeExcludedLogically = false)
        {
            if (!this.NullParameterOk)
            {
                return;
            }

            if (!this.Service.Exists(entity, KeyValuesOk, includeExcludedLogically))
            {
                var keys = entity.GetKeyValues().Select(x => $"{{{x.Property.Name}:{x.Value}}}").ToArray();
                var keyValues = string.Join(", ", keys);
                this.AddInconsistency(new EntityNotFoundFluentValidationException(keyValues));
            }
        }

        private void ThereIsOnlyOneEntity(T entity, bool includeExcludedLogically = false)
        {
            if (!this.NullParameterOk)
            {
                return;
            }

            if (this.Service.Count(entity) > 1)
            {
                var keys = entity.GetKeyValues().Select(x => $"{{{x.Property.Name}:{x.Value}}}").ToArray();
                var keyValues = string.Join(", ", keys);
                this.AddInconsistency(new EntityExistsFluentValidationException(keyValues));
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
