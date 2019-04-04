// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using System.ComponentModel.DataAnnotations;
using System.Linq;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Services;
using Fluent.Architecture.Exceptions.ValidationException;
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

        // Todo documentar
        public bool NullParameterOk { get; set; } = true;

        // Todo documentar
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
            this.EntityShouldNotExistInDatabaseBasedOnKeys(entity, false);

            this.RunTheContextValidation();
        }

        /// <summary>
        /// Validate add operation.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity)
        {
            this.ParameterMustBeInformed(entity);
            this.RequiredPropertyMustBeInformed(entity);
            this.MaxLenghtPropertyMustBeInformed(entity);
            this.AllKeysShouldBeInformedWhenThereAreMoreThanOne(entity, isUpdate: true);
            this.EntityMustExistInDatabase(entity);

            this.RunTheContextValidation();
        }

        /// <summary>
        /// Validate add operation.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Remove(T entity)
        {
            this.ParameterMustBeInformed(entity);
            this.AllKeysMustBeInformed(entity);
            this.EntityMustExistInDatabase(entity);

            this.RunTheContextValidation();
        }

        // Todo Documentar
        public virtual void Find(T entity, bool checkId = true)
        {
            this.ParameterMustBeInformed(entity);

            if (checkId)
            {
                this.AllKeysMustBeInformed(entity);
            }
            else
            {
                this.AllKeysShouldBeInformedWhenThereAreMoreThanOne(entity);
            }

            this.RunTheContextValidation();
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
                this.AddInconsistency(new NullFluentValidationException(nameof(entity)));
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

                    this.AddInconsistency(new DbFieldNotRequiredFluentValidationException(property));
                    this.KeyValuesOk = false;
                }
            }
        }

        private void EntityMustExistInDatabase(T entity)
        {
            if (!this.NullParameterOk || !this.KeyValuesOk)
            {
                return;
            }

            if (!this.Service.Exists(entity))
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
