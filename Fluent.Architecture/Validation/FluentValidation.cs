using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Fluent.Architecture.Exception.ValidationException;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Service;
using Fluent.Architecture.Extensions;

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
        protected void AddInconsistency(Exception.ValidationException.FluentValidationException ex)
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

        private void RunTheContextValidation()
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
//            AllFluentKeysMustBeInformed(entity, false);
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

        private void ParameterMustBeInformed(T entity)
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

            if (!NullParameterOk)
            {
                KeyValuesOk = false;
                return;
            }

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
        //private void EntityShouldNotExistInDatabaseBasedOnFLuentKeys(T entity)
        //{

        //    //if (!NullParameterOk || !KeyValuesOk)
        //    //{
        //    //    return;
        //    //}

        //    //var entityType = typeof(T);
        //    //var properties = entityType.GetFluentUniqueKeyProperties();
        //    //foreach (var property in properties)
        //    //{
        //    //    var value = property.GetValue(entity);
        //    //    if (value == null)
        //    //    {
        //    //        continue;
        //    //    }

        //    //    var sql = $"select * from {entityType.GetTableName()} where {property.GetColumnName()} = '{value}'";

        //    //    if (ignoreForThisElement)
        //    //    {
        //    //        var keyValues = entity.GetKeyValues().Select(x => $"{x.Key} != '{x.Value.GetDbValue()}'").ToArray();
        //    //        if (keyValues.Length > 0)
        //    //        {
        //    //            sql += " and " + string.Join(" and ", keyValues);
        //    //        }
        //    //    }

        //    //    if (Repository.ExistsSql(sql))
        //    //    {
        //    //        AddInconsistency(new UniqueKeyFluentValidationException(property.Name, value.ToString()));
        //    //    }
        //    //}
        //}

        private void AllKeysShouldBeInformedWhenThereAreMoreThanOne(T entity)
        {
            if (!NullParameterOk || !KeyValuesOk)
            {
                return;
            }

            var entityType = typeof(T);
            var properties = entityType.GetFluentUniqueKeyProperties();
            if (properties.Count > 1)
            {
                AllFluentKeysMustBeInformed(entity);
            }
            else
            {
                var property = properties.First();
                if (property.GetValue(entity).IsFluentNull())
                {

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
