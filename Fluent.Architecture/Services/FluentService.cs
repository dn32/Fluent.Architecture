// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluent.Architecture.Core.Controllers.ControllerModel;
using Fluent.Architecture.Core.Interfaces;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Exceptions.ValidationException;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Interfaces;
using Fluent.Architecture.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
        protected internal new IFluentRepository<T> Repository
        {
            get => base.Repository as IFluentRepository<T>;
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
            this.Repository = Setup.Config.Config.RepositoryFactory.Create(TransactionObjects, this);
            this.Validation = ValidationFactory.Create<T>();
            this.Validation.Init(this);
        }

        //Todo - ATENÇÃO! AO USAR ESSE MÉTODO, A OPERAÇÃO NÃO É MAIS TRANSACIONADA. REMOVER ISSO DEPOIS DE IMPLEMENTAR O MODELO DE COMPOSIÇÃO ENTRE AS ENTIDADES
        //protected void SaveChanges()
        //{
        //    TransactionObjects.Session.SaveChanges();
        //}

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
        public virtual List<TO> ListSelect<TO>(IFluentSpecification<TO> spec, FluentPagination pagination = null)
        {
            return Repository.ListSelect(spec, pagination);
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
        public virtual List<T> List(IFluentSpecification spec, FluentPagination pagination = null)
        {
            return Repository.List(spec, pagination);
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
        public virtual TO FirstOrDefaultSelect<TO>(IFluentSpecification<TO> spec)
        {
            return this.Repository.FirstOrDefaultSelect(spec);
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

        public virtual T FirstOrDefault(IFluentSpecification spec)
        {
            return Repository.FirstOrDefault(spec);
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
        public virtual int CountSelect<TO>(IFluentSpecification<TO> spec)
        {
            return this.Repository.CountSelect(spec);
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
        public virtual int Count(IFluentSpecification spec)
        {
            return this.Repository.Count(spec);
        }

        // Todo2 documentar
        public virtual int Count()
        {
            return Repository.Count();
        }

        // Todo2 documentar
        public virtual void RemoveRange(IFluentSpecification spec)
        {
            Repository.RemoveRange(spec);
        }

        public virtual void Truncate(string ERASE_ALL_DATA)
        {
            Validation.Truncate(ERASE_ALL_DATA);
            Repository.Truncate();
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
        public virtual bool Exists(ISpec spec)
        {
            return this.Repository.Exists(spec);
        }

        public virtual bool Exists(T entity, bool checkId = true, bool includeExcludedLogically = false)
        {
            return this.Repository.Exists(entity, includeExcludedLogically);
        }

        public virtual bool ExistsSelect<TO>(ISpec spec)
        {
            return this.Repository.ExistsSelect<TO>(spec);
        }

        /// <summary>
        /// Adiciona vários itens de um mesmo tipo ao banco de dados.
        /// </summary>
        /// <param name="entities">
        /// Itens a serem adicionados.
        /// </param>

        public virtual void AddRange(params T[] entities)
        {
            Validation.AddRange(entities);
            this.Repository.AddRange(entities);
        }

        /// <summary>
        /// Adiciona um item ao banco de dados.
        /// </summary>
        /// <param name="entity">
        /// Item a ser adicionado.
        /// </param>

        public virtual T Add(T entity)
        {
            this.Validation.Add(entity);
            return Repository.Add(entity);
        }

        // Todo2 documentar

        public virtual T AddOrUpdate(T entity)
        {
            Validation.AddOrUpdate(entity);
            var exists = SessionRequest.ContextFluentValidationException.Inconsistencies.Any(x => x.ExceptionType == nameof(EntityExistsFluentValidationException));
            SessionRequest.ContextFluentValidationException.Inconsistencies.Clear();

            if (exists)
            {
                return Update(entity);
            }
            else
            {
                return Add(entity);
            }
        }

        public virtual T Find(T entity, bool checkId = true)
        {
            Validation.Find(entity, checkId);
            return Repository.Find(entity);
        }

        /// <summary>
        /// Atualiza um item do banco de dados baseado em seu identificador.
        /// </summary>
        /// <param name="entity">
        /// Entidade a ser atualizada com o identificador preenchido.
        /// </param>

        public virtual T Update(T entity)
        {
            this.Validation.Update(entity);
            return this.Repository.Update(entity);
        }

        internal T UpdateAlter(UpdateAlter<T> value)
        {
            Validation.UpdateAlter(value);
            return Repository.UpdateAlter(value);
        }

        /// <summary>
        /// Atualiza vários itens do banco de dados baseado em seus identificadores.
        /// </summary>
        /// <param name="entity">
        /// Entidades a serem atualizadas com o identificador preenchido.
        /// </param>

        public virtual void UpdateRange(params T[] entities)
        {
            Validation.UpdateRange(entities);
            Repository.UpdateRange(entities);
        }

        /// <summary>
        /// Remove um item do banco de dados baseado em seu identificador.
        /// </summary>
        /// <param name="entity">
        /// Entidade a ser removida.
        /// </param>

        public virtual T Remove(T entity)
        {
            this.Validation.Remove(entity);
            return this.Repository.Remove(entity);
        }

        // Todo2 documentar

        public virtual void RemoveRange(params T[] entities)
        {
            this.Validation.RemoveRange(entities);
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