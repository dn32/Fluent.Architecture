using ClosedXML.Excel;
using Fluente.Arquitetura.Exceptions;
using Fluente.Arquitetura.Exceptions.ValidationException;
using Fluente.Arquitetura.Factory;
using Fluente.Arquitetura.Interfaces;
using Fluente.Arquitetura.Nucleo.Interfaces;
using Fluente.Arquitetura.Nucleo.Models;
using Fluente.Arquitetura.Nucleo.Util;
using Fluente.Arquitetura.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using dn32.infra.dados;

namespace Fluente.Arquitetura.Services
{
    ///<inheritdoc/>
    /// <summary>
    /// Serviço base para serviços com relacionamento direto com uma entidade.
    /// </summary>
    /// <typeparam name="T">
    /// A entidade relacionada ao serviço.
    /// </typeparam>
    public class FluenteService<T> : TransactionalService where T : EntidadeBase
    {
        ///// <summary>
        ///// O repositório do serviço.
        ///// </summary>
        protected internal new IFluenteRepository<T> Repository
        {
            get => base.Repository as IFluenteRepository<T>;
            set => base.Repository = value;
        }

        /// <summary>
        /// A validação do serviço.
        /// </summary>
        protected internal new FluenteValidation<T> Validation
        {
            get => base.Validation as FluenteValidation<T>;
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
        private void SaveChanges()
        {
            TransactionObjects.Session.SaveChanges();
        }

        protected virtual void TransformToPersist(T entity, bool? update) { }

        protected virtual void TransformToGet(T entity) { }

        protected virtual void TransformToGet<TO>(TO entity) { }

        internal virtual async Task<XLWorkbook> ImportFileStreamAsync(Stream stream)
        {
            var workbook = new XLWorkbook(stream);
            var list = DataImportationUtil.ImportFileStream<T>(workbook);

            foreach (var item in list)
            {
                try
                {
                    var entity = item.Item2;
                    Validation.ClearInconsistencies();

                    if (await ExistsAsync(entity, true, true))
                    {
                        await Validation.UpdateAsync(entity);
                        await Repository.UpdateAsync(entity);
                    }
                    else
                    {
                        await Validation.AddAsync(entity);
                        await Repository.AddAsync(entity);
                    }

                    Validation.RunTheContextValidation();

                    item.Item1.Value = "Sucess!";
                    item.Item1.Style.Font.FontColor = XLColor.FromArgb(0x04AC15);
                }
                catch (Exception ex)
                {
                    item.Item1.Style.Font.FontColor = XLColor.FromArgb(0xDC4C3F);
                    item.Item1.Value = ex.Message.Replace("* ", "").Trim();
                }
            }

            Validation.ClearInconsistencies();

            //stream.Close();
            //stream.Dispose();
            return workbook;
        }

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
        public virtual async Task<List<TO>> ListSelectAsync<TO>(IFluenteSpecification<TO> spec, FluentePaginacao pagination = null)
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
        public virtual async Task<List<T>> ListAsync(IFluenteSpecification spec, FluentePaginacao pagination = null)
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
        public virtual async Task<TO> FirstOrDefaultSelectAsync<TO>(IFluenteSpecification<TO> spec)
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

        public virtual async Task<T> FirstOrDefaultAsync(IFluenteSpecification spec)
        {
            var entity = await Repository.FirstOrDefaultAsync(spec);
            entity = Repository.Detach(entity);
            TransformToGet(entity);
            return entity;
        }

        public virtual async Task<T> SingleOrDefaultAsync(IFluenteSpecification spec)
        {
            var entity = await Repository.SingleOrDefaultAsync(spec);
            entity = Repository.Detach(entity);
            TransformToGet(entity);
            return entity;
        }

        /// IsAsNoTracking
        public virtual async Task<TO> SingleOrDefaultSelectAsync<TO>(IFluenteSpecification<TO> spec)
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
        public virtual async Task<int> CountSelectAsync<TO>(IFluenteSpecification<TO> spec) => await Repository.CountSelectAsync(spec);

        /// <summary>
        /// Retorna a quantidade de itens existentes que satisfaçam a uma especificação
        /// </summary>
        /// <param name="spec">
        /// A especificação de requisição.
        /// </param>
        /// <returns>
        /// A quantidade de itens.
        /// </returns>
        public virtual async Task<int> CountAsync(IFluenteSpecification spec) => await Repository.CountAsync(spec);

        // Todo2 documentar
        public virtual async Task<int> CountAsync() => await Repository.CountAsync();

        // Todo2 documentar
        public virtual void RemoveRange(IFluenteSpecification spec) => Repository.RemoveRange(spec);

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
            var exists = SessionRequest.ContextFluenteValidationException.Inconsistencies.RemoveAll(x => x.ExceptionType == nameof(EntityExistsFluenteValidationException)) > 0;

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

            throw new IncorrectDevelopmentException($"You can not initialize the {nameof(FluenteService<T>)}");
        }


        #endregion
    }
}