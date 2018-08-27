// ReSharper disable CommentTypo

using System.ComponentModel.DataAnnotations;
using System.Linq;
using Fluent.Architecture.Exception.ValidationException;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Validation
{
    /// <summary>
    /// A classe de validação base de todas as validações com entidade do sistema.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FluentValidation<T> where T : BaseEntity
    {
        #region INTERNAL

        //Todo documentar
        public bool NullParameterOk { get; set; } = true;

        //Todo documentar
        public bool KeyValuesOk { get; set; } = true;

        /// <summary>
        /// O repositório referente a entidade em validação.
        /// </summary>
        protected FluentRepository<T> Repository { get; set; }

        /// <summary>
        /// O serviço referente a entidade em validação.
        /// </summary>
        protected FluentService<T> Service { get; set; }

        /// <summary>
        /// Adiciona uma nova inconsistência ao contexto da requisição.
        /// </summary>
        /// <param name="ex">
        /// A inconsitência.
        /// </param>
        protected void AddInconsistency(FluentValidationException ex)
        {
            Service.SessionRequest.ContextFluentValidation.AddInconsistency(ex);
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
            Service = service;
            Repository = repository;
        }

        protected void RunTheContextValidation()
        {
            Service.SessionRequest.ContextFluentValidation.Validate();
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
            ParameterMustBeInformed(entity);
            RequiredPropertyMustBeInformed(entity);
            AllKeysShouldBeInformedWhenThereAreMoreThanOne(entity);
            EntityShouldNotExistInDatabaseBasedOnKeys(entity);

            RunTheContextValidation();
        }

        /// <summary>
        /// Validate add operation.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity)
        {
            ParameterMustBeInformed(entity);
            RequiredPropertyMustBeInformed(entity);
            AllKeysMustBeInformed(entity);
            //  AllFluentKeysMustBeInformed(entity, true);
            EntityMustExistInDatabase(entity);

            RunTheContextValidation();
        }

        /// <summary>
        /// Validate add operation.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Remove(T entity)
        {
            ParameterMustBeInformed(entity);
            AllKeysMustBeInformed(entity);
            EntityMustExistInDatabase(entity);

            RunTheContextValidation();
        }

        //Todo Documentar
        public virtual void Find(T entity)
        {
            ParameterMustBeInformed(entity);
            AllKeysMustBeInformed(entity);

            RunTheContextValidation();
        }

        public virtual void PropagateService(string methodName, object[] parameters)
        {
            ParameterMustBeInformed(parameters);

            if (NullParameterOk)
            {
                foreach (var parameter in parameters)
                {
                    if (parameter == null)
                    {
                        AddInconsistency(new FluentParameterValidationException(nameof(parameters), "No propagation parameter can be null."));
                    }
                }
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
                AddInconsistency(new NullParameterFluentValidationException(nameof(entity)));
                NullParameterOk = false;
                return;
            }

            NullParameterOk = true;
        }

        private void RequiredPropertyMustBeInformed(T entity)
        {
            if (!NullParameterOk)
            {
                return;
            }

            var properties = typeof(T).GetPropertiesByAttribute<RequiredAttribute>();
            foreach (var property in properties)
            {
                if (property.GetValue(entity).IsFluentNull())
                {
                    AddInconsistency(new PropertyRequiredFluentValidationException(property.Name));
                }
            }
        }

        private void AllKeysMustBeInformed(T entity)
        {
            if (!NullParameterOk)
            {
                KeyValuesOk = false;
                return;
            }

            KeyValuesOk = true;

            var keyValues = entity.GetKeyValues();
            foreach (var key in keyValues)
            {
                if (key.Value.IsFluentNull())
                {
                    AddInconsistency(new PropertyRequiredFluentValidationException(key.Property.Name));
                    KeyValuesOk = false;
                }
            }
        }

        private void AllFluentKeysMustBeInformed(T entity)
        {
            KeyValuesOk = true;

            var properties = entity.GetType().GetFluentUniqueKeyProperties();
            foreach (var property in properties)
            {
                if (property.GetValue(entity).IsFluentNull())
                {
                    AddInconsistency(new PropertyRequiredFluentValidationException(property.Name));
                    KeyValuesOk = false;
                }
            }
        }

        private void AllKeysShouldBeInformedWhenThereAreMoreThanOne(T entity)
        {
            if (!NullParameterOk || !KeyValuesOk)
            {
                return;
            }

            var entityType = typeof(T);
            var properties = entityType.GetKeyProperties();
            if (properties.Count > 1)
            {
                AllFluentKeysMustBeInformed(entity);
            }
            else
            {
                var property = properties.First();
                if (!property.GetValue(entity).IsFluentNull())
                {
                    throw new FluentPropertyValidationException(property.Name, "The key must not be entered for this operation.");
                }
            }
        }

        private void EntityMustExistInDatabase(T entity)
        {
            if (!NullParameterOk || !KeyValuesOk)
            {
                return;
            }

            if (!Repository.Exists(entity))
            {
                var keys = entity.GetKeyValues().Select(x => $"{{{x.Property.Name}:{x.Value}}}").ToArray();
                var keyValues = string.Join(", ", keys);
                AddInconsistency(new EntityNotFoundFluentValidationException(keyValues));
            }
        }

        private void EntityShouldNotExistInDatabaseBasedOnKeys(T entity)
        {
            if (!NullParameterOk || !KeyValuesOk)
            {
                return;
            }

            if (Repository.Exists(entity))
            {
                var keys = entity.GetKeyValues().Select(x => $"{{{x.Property.Name}:{x.Value}}}").ToArray();
                var keyValues = string.Join(", ", keys);
                AddInconsistency(new EntityExistsFluentValidationException(keyValues));
            }
        }

        #endregion
    }
}
