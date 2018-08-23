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
        protected void AddInconsistency(FluentValidationtException ex)
        {
            Service.SessionRequest.ContextValidation.AddInconsistency(ex);
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
            Service.SessionRequest.ContextValidation.Validate();
        }

        #endregion

        /// <summary>
        /// Validate add operation.
        /// </summary>
        /// <param name="entity">
        /// A entidade a ser validada.
        /// </param>
        public void Add(T entity)
        {
            ValidateNullParameter(entity);
            ValidateRequiredProperty(entity);
            ValidateKeylessEntity();
            //ValidIfTheKeyPropertyHasValue(entity); chaves compostas dever ter valor informado. Melhorar esse tratamento
            ValidadeFluentUnicKey(entity, false);
            ValidatetEntityExists(entity);

            RunTheContextValidation();
        }

        /// <summary>
        /// Validate add operation.
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            ValidateNullParameter(entity);
            ValidateRequiredProperty(entity);
            ValidateKeylessEntity();
            ValidIfTheKeyPropertyHasNoValue(entity);
            ValidadeFluentUnicKey(entity, true);
            ValidateNotEntityExists(entity);

            RunTheContextValidation();
        }

        /// <summary>
        /// Validate add operation.
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(T entity)
        {
            ValidateNullParameter(entity);
            ValidateKeylessEntity();
            ValidIfTheKeyPropertyHasNoValue(entity);
            ValidateNotEntityExists(entity);

            RunTheContextValidation();
        }

        //Todo Documentar
        public void Find(T entity)
        {
            ValidateNullParameter(entity);
            ValidateKeylessEntity();
            ValidIfTheKeyPropertyHasNoValue(entity);

            RunTheContextValidation();
        }

        #region VALIDATIONS

        private void ValidateRequiredProperty(T entity)
        {
            if (entity == null)
            {
                return;
            }

            var properties1 = typeof(T).GetKeyProperties();
            var properties2 = typeof(T).GetPropertiesByAttribute<RequiredAttribute>();
            properties1.AddRange(properties2);
            foreach (var property in properties1)
            {
                if (property.GetValue(entity) == null)
                {
                    AddInconsistency(new PropertyRequiredFluentValidationtException(property.Name));
                }
            }
        }

        private void ValidateNullParameter(T entity)
        {
            if (entity == null)
            {
                AddInconsistency(new NullParameterFluentValidationtException(nameof(entity)));
            }
        }

        private void ValidateKeylessEntity()
        {
            if (typeof(T).GetKeyProperties().Count == 0)
            {
                AddInconsistency(new KeylessEntityFluentValidationtException(typeof(T)));
            }
        }

        //For add chave deve ser 0
        private void ValidIfTheKeyPropertyHasValue(T entity)
        {
            if (entity == null)
            {
                return;
            }

            var keyValues = entity.GetKeyValues();
            foreach (var key in keyValues)
            {
                if (key.Value != 0)
                {
                    AddInconsistency(new PropertyNotNullFluentValidationtException(key.Key));
                }
            }
        }

        //For Update chave não pode ser 0
        private void ValidIfTheKeyPropertyHasNoValue(T entity)
        {
            if (entity == null)
            {
                return;
            }

            var keyValues = entity.GetKeyValues();
            foreach (var key in keyValues)
            {
                if (key.Value == 0)
                {
                    AddInconsistency(new PropertyNullFluentValidationtException(key.Key));
                }
            }
        }

        private void ValidadeFluentUnicKey(T entity, bool ignoreForThisElement)
        {
            if (entity == null)
            {
                return;
            }

            var entityType = typeof(T);
            var properties = entityType.GetFluentUnicKeyProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(entity);
                if (value == null)
                {
                    continue;
                }

                var sql = $"select * from {entityType.GetTableName()} where {property.GetColumnName()} = '{value}'";

                if (ignoreForThisElement)
                {
                    var keyValues = entity.GetKeyValues().Select(x => $"{x.Key} != {x.Value}").ToArray();
                    if (keyValues.Length > 0)
                    {
                        sql += " and " + string.Join(" and ", keyValues);
                    }
                }

                if (Repository.ExistsSql(sql))
                {
                    AddInconsistency(new UnicKeyFluentValidationtException(property.Name, value.ToString()));
                }
            }
        }

        //No update deve existir, se não existir, da erro
        private void ValidateNotEntityExists(T entity)
        {
            if (entity == null)
            {
                return;
            }

            if (!Repository.Exists(entity))
            {
                var keys = entity.GetKeyValues().Select(x => $"{{{x.Key}:{x.Value}}}").ToArray();
                var keyValues = string.Join(", ", keys);
                AddInconsistency(new EntityNotFoundFluentValidationtException(keyValues));
            }
        }

        //Todo  - Documentar
        //No insert não deve existir. Se existir, da erro
        private void ValidatetEntityExists(T entity)
        {
            if (entity == null)
            {
                return;
            }

            if (Repository.Exists(entity))
            {
                var keys = entity.GetKeyValues().Select(x => $"{{{x.Key}:{x.Value}}}").ToArray();
                var keyValues = string.Join(", ", keys);
                AddInconsistency(new EntityExistsFluentValidationtException(keyValues));
            }
        }

        #endregion
    }
}
