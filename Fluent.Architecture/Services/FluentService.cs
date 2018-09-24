// ReSharper disable CommentTypo

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Interfaces;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Validation;

namespace Fluent.Architecture.Services
{
    ///<inheritdoc/>
    /// <summary>
    /// Serviço base para serviços com relacionamento direto com uma entidade.
    /// </summary>
    /// <typeparam name="T">
    /// A entidade relacionada ao serviço.
    /// </typeparam>
    public class FluentService<T> : TransactionalService where T : BaseEntity
    {
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
        protected internal new FluentValidation<T> Validation
        {
            get => base.Validation as FluentValidation<T>;
            set => base.Validation = value;
        }

        protected internal override void SetUserSession(UserSessionRequest sessionRequest)
        {
            base.SetUserSession(sessionRequest);

            ValidateInit();
            this.Repository = RepositoryFactory<T>.Create(this.TransactionObjects, this);
            this.Validation = ValidationFactory.Create<T>();
            this.Validation.Init(this, this.Repository);
        }

        #region PROPAGATION

        // Todo Documenta após a organização desses itens.
        public virtual object PropagateService(string methodName, object[] parameters)
        {
          //  base.Repository = null;

            Validation.PropagateService(methodName, parameters);

            var validationMethod = Validation.GetType().GetMethodWithoutAmbiguity(methodName, parameters, new[] { typeof(T) });
            var serviceMethod = GetType().GetMethodWithoutAmbiguity(methodName, parameters, new[] { typeof(T) });

            if (validationMethod != null)
            {
                validationMethod.FluenInvoke(Validation, parameters);
            }

            if (serviceMethod != null)
            {
                return serviceMethod.FluenInvoke(this, parameters);
            }

            var repositoryMethod = Repository.GetType().GetMethodWithoutAmbiguity(methodName, parameters, new[] { typeof(T) });
            if (repositoryMethod != null)
            {
                return repositoryMethod.FluenInvoke(Repository, parameters);
            }

            throw new IncorrectDevelopmentException($"The {methodName} method was not found in the service {this.GetType().BaseType} and repository {Repository.GetType()}");
        }

        #endregion

        #region PASSAGEM DIRETA PARA O REPOSITÓRIO

        // Todo - Esses métoso são redundantes. Crier um mecanismo para não necessitar reencrever essas chamadas.
        /// <summary>
        /// Executa uma solicitação baseada em uma especificação e retorna uma lista paginada de resultados.
        /// </summary>
        /// <typeparam name="TO">
        /// O tipo de saida desejada. Deve ser o mesmo definido na saida da especificação.
        /// </typeparam>
        /// <param name="spec">
        /// A especificação de requisição.
        /// </param>
        /// <param name="pagination">
        /// A paginação desejada.
        /// </param>
        /// <returns>
        /// A lista paginada de resultados.
        /// </returns>
        [Propagate]
        public virtual List<TO> List<TO>(IFluentSpecification<TO> spec, FluentPagination pagination = null)
        {
            return Repository.List(spec, pagination);
        }

        /// <summary>
        /// Executa uma solicitação baseada em uma especificação e retorna uma lista paginada de resultados.
        /// </summary>
        /// <param name="spec">
        /// A especificação de requisição.
        /// </param>
        /// <param name="pagination">
        /// A paginação desejada.
        /// </param>
        /// <returns>
        /// A lista paginada de resultados.
        /// </returns>
        [Propagate]
        public virtual List<T> List(IFluentSpecification spec, FluentPagination pagination = null)
        {
            return this.Repository.List(spec, pagination);
        }

        /// <summary>
        /// Executa uma solicitação baseada em uma especificação e retorna um resultado ou nulo quando a consulta não é satisfeita.
        /// </summary>
        /// <typeparam name="TO">
        /// O tipo de saida desejada. Deve ser o mesmo definido na saida da especificação.
        /// </typeparam>
        /// <param name="spec">
        /// A especificação de requisição.
        /// </param>
        /// <returns>
        /// O item referente à consulta ou nulo.
        /// </returns>
        [Propagate]
        public virtual TO FirstOrDefault<TO>(IFluentSpecification<TO> spec)
        {
            return this.Repository.FirstOrDefault(spec);
        }

        /// <summary>
        /// Executa uma solicitação baseada em uma especificação e retorna um resultado ou nulo quando a consulta não é satisfeita.
        /// </summary>
        /// <param name="spec">
        /// A especificação de requisição.
        /// </param>
        /// <returns>
        /// O item referente à consulta ou nulo.
        /// </returns>
        [Propagate]
        public virtual T FirstOrDefault(IFluentSpecification spec)
        {
            return this.Repository.FirstOrDefault(spec);
        }

        //Todo Doc
        [Propagate]
        public virtual T FirstOrDefault()
        {
            return this.Repository.FirstOrDefault();
        }

        /// <summary>
        /// Retorna a quantidade de itens existentes que satisfaçam a uma especificação
        /// </summary>
        /// <typeparam name="TO">
        /// O tipo de saida desejada. Deve ser o mesmo definido na saida da especificação.
        /// </typeparam>
        /// <param name="spec">
        /// A especificação de requisição.
        /// </param>
        /// <returns>
        /// A quantidade de itens.
        /// </returns>
        [Propagate]
        public virtual int Count<TO>(IFluentSpecification<TO> spec)
        {
            return this.Repository.Count(spec);
        }

        /// <summary>
        /// Retorna a quantidade de itens existentes que satisfaçam a uma especificação
        /// </summary>
        /// <param name="spec">
        /// A especificação de requisição.
        /// </param>
        /// <returns>
        /// A quantidade de itens.
        /// </returns>
        [Propagate]
        public virtual int Count(IFluentSpecification spec)
        {
            return this.Repository.Count(spec);
        }

        // Todo documentar
        [Propagate]
        public virtual int Count()
        {
            return Repository.Count();
        }

        // Todo documentar
        [Propagate]
        public virtual void RemoveRange(IFluentSpecification spec)
        {
            this.Repository.RemoveRange(spec);
        }

        /// <summary>
        /// Avalia se um item existe no banco de dados, baseado em uma especificação.
        /// </summary>
        /// <param name="spec">
        /// A especificação de requisição.
        /// </param>
        /// <returns>
        /// Se o item existe ou não.
        /// </returns>
        [Propagate]
        public virtual bool Exists(IFluentSpecification spec)
        {
            return this.Repository.Exists(spec);
        }

        /// <summary>
        /// Avalia se um item existe no banco de dados, baseado em uma especificação.
        /// </summary>
        /// <param name="spec">
        /// A especificação de requisição.
        /// </param>
        /// <returns>
        /// Se o item existe ou não.
        /// </returns>
        [Propagate]
        public virtual bool Exists<TO>(IFluentSpecification<TO> spec)
        {
            return this.Repository.Exists(spec);
        }

        /// <summary>
        /// Adiciona vários itens de um mesmo tipo ao banco de dados.
        /// </summary>
        /// <param name="entities">
        /// Itens a serem adicionados.
        /// </param>
        [Propagate]
        public virtual void AddRange(params T[] entities)
        {
            entities.ToList().ForEach(this.Validation.Add);
            this.Repository.AddRange(entities);
        }

        /// <summary>
        /// Adiciona um item ao banco de dados.
        /// </summary>
        /// <param name="entity">
        /// Item a ser adicionado.
        /// </param>
        [Propagate]
        public virtual T Add(T entity)
        {
            this.Validation.Add(entity);
            return this.Repository.Add(entity);
        }

        // Todo documentar
        [Propagate]
        public virtual T Find(T entity)
        {
            this.Validation.Find(entity);
            return this.Repository.Find(entity);
        }

        /// <summary>
        /// Atualiza um item do banco de dados baseado em seu identificador.
        /// </summary>
        /// <param name="entity">
        /// Entidade a ser atualizada com o identificador preenchido.
        /// </param>
        [Propagate]
        public virtual T Update(T entity)
        {
            this.Validation.Update(entity);
            return this.Repository.Update(entity);
        }

        /// <summary>
        /// Remove um item do banco de dados baseado em seu identificador.
        /// </summary>
        /// <param name="entity">
        /// Entidade a ser removida.
        /// </param>
        [Propagate]
        public virtual T Remove(T entity)
        {
            this.Validation.Remove(entity);
            return this.Repository.Remove(entity);
        }

        // Todo documentar
        [Propagate]
        public virtual void RemoveRange(params T[] entities)
        {
            foreach (var entity in entities)
            {
                this.Validation.Remove(entity);
            }

            this.Repository.RemoveRange(entities);
        }

        #endregion

        #region PRIVATE

        /// <summary>
        /// Valida a tentativa de instância de um serviço.
        /// </summary>
        private static void ValidateInit()
        {
            var mth = new StackTrace().GetFrame(2).GetMethod();
            var name = mth.ReflectedType?.Name;
            if (name == nameof(ServiceFactory))
            {
                return;
            }

            throw new IncorrectDevelopmentException($"You can not initialize the {nameof(FluentService<T>)}");
        }

        #endregion
    }
}