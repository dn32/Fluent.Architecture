// ReSharper disable CommentTypo

using System.ComponentModel.DataAnnotations;
using System.Linq;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Services;
using Fluent.Architecture.Exceptions.ValidationException;

namespace Fluent.Architecture.Validation
{
    /// <summary>
    /// A classe de validação base de todas as validações com entidade do sistema.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FluentValidation<T> : TransactionalValidation where T : BaseEntity
    {
        #region INTERNAL

        ///// <summary>
        ///// O repositório do serviço.
        ///// </summary>
        protected internal new FluentRepository<T> Repository
        {
            get => base.Repository as FluentRepository<T>;
            set => base.Repository = value;
        }

        /// <summary>
        /// A validação do serviço.
        /// </summary>
        protected internal new FluentService<T> Service
        {
            get => base.Service as FluentService<T>;
            set => base.Service = value;
        }

        // Todo documentar
        public bool NullParameterOk { get; set; } = true;

        // Todo documentar
        public bool KeyValuesOk { get; set; } = true;

        /// <summary>
        /// Adiciona uma nova inconsistência ao contexto da requisição.
        /// </summary>
        /// <param name="ex">
        /// A inconsitência.
        /// </param>
        protected void AddInconsistency(FluentValidationException ex)
        {
            this.Service.SessionRequest.ContextFluentValidationException.AddInconsistency(ex);
        }

        /// <summary>
        /// Inicializa a classe preenchendo suas dependências.
        /// </summary>
        /// <param name="service">
        /// O serviço que a validação representa.
        /// </param>
        /// <param name="repository">
        /// O repositório que a validação representa.
        /// </param>
        internal void Init(FluentService<T> service, FluentRepository<T> repository)
        {
            this.Service = service;
            this.Repository = repository;
        }

        protected void RunTheContextValidation()
        {
            this.Service.SessionRequest.ContextFluentValidationException.Validate();
        }

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
            this.AllKeysShouldBeInformedWhenThereAreMoreThanOne(entity);
            this.EntityShouldNotExistInDatabaseBasedOnKeys(entity);

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
        public virtual void Find(T entity)
        {
            this.ParameterMustBeInformed(entity);
            this.AllKeysMustBeInformed(entity);

            this.RunTheContextValidation();
        }

        public virtual void PropagateService(string methodName, object[] parameters)
        {
            this.ParameterMustBeInformed(parameters);

            if (this.NullParameterOk)
            {
                foreach (var parameter in parameters)
                {
                    if (parameter == null)
                    {
                        this.AddInconsistency(new FluentParameterValidationException(nameof(parameters), "No propagation parameter can be null."));
                    }
                }
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
                this.AddInconsistency(new NullParameterFluentValidationException(nameof(entity)));
                this.NullParameterOk = false;
                return;
            }

            this.NullParameterOk = true;
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
                    this.AddInconsistency(new PropertyRequiredFluentValidationException(property.Name));
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
                    this.AddInconsistency(new PropertyRequiredFluentValidationException(property.Name));
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

                    this.AddInconsistency(new PropertyRequiredFluentValidationException(property.Name));
                    this.KeyValuesOk = false;
                }
                else
                {
                    if (isUpdate)
                    {
                        return;
                    }

                    this.AddInconsistency(new FluentPropertyValidationException(property.Name, "The key must not be entered for this operation."));
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

            if (!this.Repository.Exists(entity))
            {
                var keys = entity.GetKeyValues().Select(x => $"{{{x.Property.Name}:{x.Value}}}").ToArray();
                var keyValues = string.Join(", ", keys);
                this.AddInconsistency(new EntityNotFoundFluentValidationException(keyValues));
            }
        }

        private void EntityShouldNotExistInDatabaseBasedOnKeys(T entity)
        {
            if (!this.NullParameterOk || !this.KeyValuesOk)
            {
                return;
            }

            if (this.Repository.Exists(entity))
            {
                var keys = entity.GetKeyAndFluentUniqueKeyValues().Select(x => $"{{{x.Property.Name}:{x.Value}}}").ToArray();
                var keyValues = string.Join(", ", keys);
                this.AddInconsistency(new EntityExistsFluentValidationException(keyValues));
            }
        }

        #endregion
    }
}
