// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluent.Architecture.Core.Interfaces;
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Exceptions.ValidationException;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Interfaces;
using Fluent.Architecture.Validation;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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

        protected virtual void TransformToPersist(T entity, bool? update) { }

        protected virtual void TransformToGet(T entity) { }

        protected virtual void TransformToGet<TO>(TO entity) { }

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
        public virtual async Task<List<TO>> ListSelectAsync<TO>(IFluentSpecification<TO> spec, FluentPagination pagination = null)
        {
            var list = await Repository.ListSelectAsync(spec, pagination);
            list.ForEach(x => Repository.Detach(x));
            list.ForEach(TransformToGet);
            return list;
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
        public virtual async Task<List<T>> ListAsync(IFluentSpecification spec, FluentPagination pagination = null)
        {
            var list = await Repository.ListAsync(spec, pagination);
            list.ForEach(x => Repository.Detach(x));
            list.ForEach(TransformToGet);
            return list;
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
        public virtual async Task<TO> FirstOrDefaultSelectAsync<TO>(IFluentSpecification<TO> spec)
        {
            var entity = await Repository.FirstOrDefaultSelectAsync(spec);
            entity = Repository.Detach(entity);
            TransformToGet(entity);
            return entity;
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

        public virtual async Task<T> FirstOrDefaultAsync(IFluentSpecification spec)
        {
            var entity = await Repository.FirstOrDefaultAsync(spec);
            entity = Repository.Detach(entity);
            TransformToGet(entity);
            return entity;
        }

        public virtual async Task<T> SingleOrDefaultAsync(IFluentSpecification spec)
        {
            var entity = await Repository.SingleOrDefaultAsync(spec);
            entity = Repository.Detach(entity);
            TransformToGet(entity);
            return entity;
        }

        /// IsAsNoTracking
        public virtual async Task<TO> SingleOrDefaultSelectAsync<TO>(IFluentSpecification<TO> spec)
        {
            var entity = await Repository.SingleOrDefaultSelectAsync(spec);
            entity = Repository.Detach(entity);
            TransformToGet(entity);
            return entity;
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
        public virtual async Task<int> CountSelectAsync<TO>(IFluentSpecification<TO> spec) => await Repository.CountSelectAsync(spec);

        /// <summary>
        /// Retorna a quantidade de itens existentes que satisfaçam a uma especificação
        /// </summary>
        /// <param name="spec">
        /// A especificação de requisição.
        /// </param>
        /// <returns>
        /// A quantidade de itens.
        /// </returns>
        public virtual async Task<int> CountAsync(IFluentSpecification spec) => await Repository.CountAsync(spec);

        // Todo2 documentar
        public virtual async Task<int> CountAsync() => await Repository.CountAsync();

        // Todo2 documentar
        public virtual void RemoveRange(IFluentSpecification spec) => Repository.RemoveRange(spec);

        public virtual async Task TruncateAsync(string ERASE_ALL_DATA)
        {
            Validation.Truncate(ERASE_ALL_DATA);
            await Repository.TruncateAsync();
        }

        public virtual async Task<bool> ExistsAsync(ISpec spec) => await Repository.ExistsAsync(spec);

        public virtual async Task<bool> ExistsAsync(T entity, bool checkId = true, bool includeExcludedLogically = false) => await Repository.ExistsAsync(entity, includeExcludedLogically);

        public virtual async Task<bool> ExistsOnlyOneAsync(T entity, bool includeExcludedLogically = false) => await Repository.ExistsOnlyOneAsync(entity, includeExcludedLogically).ConfigureAwait(false);

        public virtual async Task<int> CountAsync(T entity, bool includeExcludedLogically = false) => await Repository.CountAsync(entity, includeExcludedLogically);

        public virtual async Task<bool> ExistsSelectAsync<TO>(ISpec spec) => await Repository.ExistsSelectAsync<TO>(spec);

        /// <summary>
        /// Adiciona vários itens de um mesmo tipo ao banco de dados.
        /// </summary>
        /// <param name="entities">
        /// Itens a serem adicionados.
        /// </param>

        public virtual async Task AddRangeAsync(params T[] entities)
        {
            foreach (var item in entities) { TransformToPersist(item, false); }
            await Validation.AddRangeAsync(entities);
            await Repository.AddRangeAsync(entities);
        }

        /// <summary>
        /// IsAsNoTracking
        /// Adiciona um item ao banco de dados.
        /// </summary>
        /// <param name="entity">
        /// Item a ser adicionado.
        /// </param>

        public virtual async Task<T> AddAsync(T entity)
        {
            TransformToPersist(entity, false);

            if (await ExistsAsync(entity, true, true))
            {
                return await UpdateAsync(entity); // Restore deleted
            }
            else
            {
                await Validation.AddAsync(entity);
                return await Repository.AddAsync(entity);
                //return Repository.Detach(entity);//Detach aqui não permite salvar a entidade
            }
        }

        // Todo2 documentar

        public virtual async Task<T> AddOrUpdateAsync(T entity)
        {
            TransformToPersist(entity, null);

            var anotherServices = await Validation.AddOrUpdateAsync(entity);
            var exists = SessionRequest.ContextFluentValidationException.Inconsistencies.RemoveAll(x => x.ExceptionType == nameof(EntityExistsFluentValidationException)) > 0;

            Validation.RunTheContextValidation(anotherServices);

            if (exists)
            {
                return await UpdateAsync(entity);
            }
            else
            {
                return await AddAsync(entity);
            }
        }

        /// IsAsNoTracking
        public virtual async Task<T> FindAsync(T entity, bool checkId = true, bool detach = true)
        {
            Validation.Find(entity, checkId);
            entity = await Repository.FindAsync(entity);
            entity = detach ? Repository.Detach(entity) : entity;
            TransformToGet(entity);
            return entity;
        }

        /// <summary>
        /// IsAsNoTracking
        /// Atualiza um item do banco de dados baseado em seu identificador.
        /// </summary>
        /// <param name="entity">
        /// Entidade a ser atualizada com o identificador preenchido.
        /// </param>

        public virtual async Task<T> UpdateAsync(T entity)
        {
            TransformToPersist(entity, true);
            await Validation.UpdateAsync(entity);
            return await Repository.UpdateAsync(entity);
            //return Repository.Detach(entity);//Detach aqui não permite salvar a entidade
        }

        /// <summary>
        /// Atualiza vários itens do banco de dados baseado em seus identificadores.
        /// </summary>
        /// <param name="entity">
        /// Entidades a serem atualizadas com o identificador preenchido.
        /// </param>

        public virtual async Task UpdateRangeAsync(params T[] entities) => await UpdateRangeAsync(entities);

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            foreach (var item in entities) { TransformToPersist(item, true); }
            await Validation.UpdateRangeAsync(entities);
            await Repository.UpdateRangeAsync(entities);
        }

        /// <summary>
        /// IsAsNoTracking
        /// Remove um item do banco de dados baseado em seu identificador.
        /// </summary>
        /// <param name="entity">
        /// Entidade a ser removida.
        /// </param>
        public virtual async Task<T> RemoveAsync(T entity)
        {
            await Validation.RemoveAsync(entity);
            return await Repository.RemoveAsync(entity);
            //return Repository.Detach(entity);//Detach aqui não permite salvar a entidade
        }


        public virtual async Task RemoveRangeAsync(params T[] entities)
        {
            await Validation.RemoveRangeAsync(entities);
            await Repository.RemoveRangeAsync(entities);
        }

        #endregion

        #region PRIVATE

        /// <summary>
        /// Valida a tentativa de instância de um serviço.
        /// </summary>
        private static void ValidateInit()
        {
            var mth = new StackTrace()?.GetFrame(2)?.GetMethod() ?? null;
            var name = mth?.ReflectedType?.Name;
            if (name == nameof(ServiceFactory))
            {
                return;
            }

            throw new IncorrectDevelopmentException($"You can not initialize the {nameof(FluentService<T>)}");
        }


        #endregion
    }
}