// ReSharper disable CommentTypo

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Exception;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Util;
using Fluent.Architecture.Validation;

namespace Fluent.Architecture.Service
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
        /// <summary>
        /// O repositório do serviço.
        /// </summary>
        // protected internal FluentRepository<T> Repository { get; set; }
        protected internal FluentRepository<T> Repository;

        /// <summary>
        /// A validação do serviço.
        /// </summary>
        protected internal FluentValidation<T> Validation { get; set; }

        protected internal override void SetUserSession(UserSessionRequest sessionRequest)
        {
            base.SetUserSession(sessionRequest);

            ValidateInit();
            Repository = RepositoryFactory<T>.Create(TransactionObjects, this);
            Validation = ValidationFactory.Create<T>();
            Validation.Init(this, Repository);
        }

#if PROPAGATION
        // Todo Documenta após a organização desses itens.

        //[NotPropagate]
        //public virtual object PropagateService(BaseSpecification spec)
        //{
        //    return PropagateService(string.Empty, spec);
        //}

        //[NotPropagate]
        //public virtual object PropagateService(params object[] parameters)
        //{
        //    return PropagateService(string.Empty, parameters);
        //}

        //[NotPropagate]
        //public virtual object PropagateService(string methodName, params object[] parameters)
        //{
        //    CallValidationPerPropagation<T>(methodName, this, parameters);
        //    return GlobalUtil.GetPropagationMethod<T, T>(methodName, Repository, GetType(), parameters);
        //}

        //[NotPropagate]
        //public virtual object PropagateService<T2>(string methodName, FluentServiceController<TransactionalService> _this, params object[] parameters) where T2 : BaseEntity
        //{
        //    // Todo - Note que nesse ponto, T agora é TS. Essas operações abaixo devem ser ajustadas para atender a TS que não tem entidade
        //    //CallValidationPerPropagation<T>(methodName, _this, parameters);
        //    //return GlobalUtil.GetPropagationMethod<T, T>(methodName, Repository, GetType(), parameters);
        //    return null;
        //}

        //// If this method is internal the interceptor does not pick up and a number of problems will be noticed with sessionRequest control.
        //[NotPropagate]
        //public virtual object PropagateService<T2>(FluentController<T> _this, params object[] parameters) where T2 : BaseEntity
        //{
        //    CallValidationPerPropagation<T>(string.Empty, _this, parameters);
        //    return GlobalUtil.GetPropagationMethod<T, T2>(string.Empty, Repository, _this.GetType(), parameters);
        //}

        //[NotPropagate]
        //public virtual object PropagateService<T2>(params object[] parameters)
        //{
        //    CallValidationPerPropagation<T>(string.Empty, this, parameters);
        //    return GlobalUtil.GetPropagationMethod<T, T2>(string.Empty, Repository, GetType(), parameters);
        //}

        //[NotPropagate]
        //public virtual object PropagateService<T2>(string methodName, params object[] parameters)
        //{
        //    CallValidationPerPropagation<T>(methodName, this, parameters);
        //    return GlobalUtil.GetPropagationMethod<T, T2>(methodName, Repository, GetType(), parameters);
        //}

        //[NotPropagate]
        //private void CallValidationPerPropagation<T2>(string methodName, object _this, params object[] parameters)
        //{
        //    GlobalUtil.GetPropagationMethod<T, T2>(methodName, Validation, _this.GetType(), parameters, true);
        //}

        [NotPropagate]
        public virtual object PropagateService<T2>(string methodName, params object[] parameters)
        {
            Validation.PropagateService<T2>(methodName, parameters);

            var type = GetType();
            var parameterTypes = parameters.Select(x => x.GetType()).ToArray();
            var serviceMethod = type.GetMethod(methodName, parameterTypes);
            if (serviceMethod != null)
            {
                return serviceMethod.Invoke(this, parameters);
            }

            var validationMethod = Validation.GetType().GetMethod(methodName, parameterTypes);
            if (validationMethod != null)
            {
                validationMethod.Invoke(Validation, parameters);
            }

            var repositoryMethod = Repository.GetType().GetMethod(methodName, parameterTypes);
            if (repositoryMethod != null)
            {
                return repositoryMethod.Invoke(Repository, parameters);
            }

            var serviceName = type.GetFluentEntityType();
            var repositoryName = Repository.GetType().GetFluentEntityType();
            throw new IncorrectDevelopmentException($"The {methodName} method was not found in the service {serviceName} and repository {repositoryName}");
        }

#endif

        #region PASSAGEM DIRETA PARA O REPOSITÓRIO
        //Todo - Esses métoso são redundantes. Crier um mecanismo para não necessitar reencrever essas chamadas.
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
        public virtual List<TO> Spec<TO>(FluentSelectSpecification<T, TO> spec, FluentPagination pagination = null)
        {
            return Repository.Spec(spec, pagination);
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
        public virtual List<T> Spec(FluentSpecification<T> spec, FluentPagination pagination = null)
        {
            return Repository.Spec(spec, pagination);
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
        public virtual TO SpecOne<TO>(FluentSelectSpecification<T, TO> spec)
        {
            return Repository.SpecOne(spec);
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
        public virtual T SpecOne(FluentSpecification<T> spec)
        {
            return Repository.SpecOne(spec);
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
        public virtual int Count<TO>(FluentSelectSpecification<T, TO> spec)
        {
            return Repository.Count(spec);
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
        public virtual int Count(FluentSpecification<T> spec)
        {
            return Repository.Count(spec);
        }

        //Todo documentar
        [Propagate]
        public virtual void RemoveRange(FluentSpecification<T> spec)
        {
            Repository.RemoveRange(spec);
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
        public virtual bool Exists(FluentSpecification<T> spec)
        {
            return Repository.Exists(spec);
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
        public virtual bool Exists<TO>(FluentSelectSpecification<T, TO> spec)
        {
            return Repository.Exists(spec);
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
            entities.ToList().ForEach(Validation.Add);
            Repository.AddRange(entities);
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
            Validation.Add(entity);
            return Repository.Add(entity);
        }

        //Todo documentar
        [Propagate]
        public virtual T Find(T entity)
        {
            Validation.Find(entity);
            return Repository.Find(entity);
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
            Validation.Update(entity);
            return Repository.Update(entity);
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
            Validation.Remove(entity);
            return Repository.Remove(entity);
        }

        //Todo documentar
        [Propagate]
        public virtual void RemoveRange(params T[] entities)
        {
            foreach (var entity in entities)
            {
                Validation.Remove(entity);
            }

            Repository.RemoveRange(entities);
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