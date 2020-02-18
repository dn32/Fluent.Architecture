using ClosedXML.Excel;
using dn32.infra.Exceptions;
using dn32.infra.Exceptions.ValidationException;
using dn32.infra.Factory;
using dn32.infra.Interfaces;
using dn32.infra.Nucleo.Interfaces;
using dn32.infra.Nucleo.Models;
using dn32.infra.Nucleo.Util;
using dn32.infra.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using dn32.infra.dados;

namespace dn32.infra.Services
{
    ///<inheritdoc/>
    /// <summary>
    /// Serviço base para serviços com relacionamento direto com uma entidade.
    /// </summary>
    /// <typeparam Nome="T">
    /// A entidade relacionada ao serviço.
    /// </typeparam>
    public class DnService<T> : TransactionalService where T : EntidadeBase
    {
        ///// <summary>
        ///// O repositório do serviço.
        ///// </summary>
        protected internal new IDnRepository<T> Repository
        {
            get => base.Repository as IDnRepository<T>;
            set => base.Repository = value;
        }

        /// <summary>
        /// A validação do serviço.
        /// </summary>
        protected internal new DnValidation<T> Validation
        {
            get => base.Validation as DnValidation<T>;
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
        /// <typeparam Nome="TO">
        /// O tipo de saida desejada. Deve ser o mesmo definido na saida da especificação.
        /// </typeparam>
        /// <param Nome="spec">
        /// A especificação de requisição.
        /// </param>
        /// <param Nome="pagination">
        /// A paginação desejada.
        /// </param>
        /// <returns>
        /// A lista paginada de resultados.
        /// </returns>    
        public virtual async Task<List<TO>> ListSelectAsync<TO>(IDnSpecification<TO> spec, DnPaginacao pagination = null)
        {
            var list = await Repository.ListSelectAsync(spec, pagination);
            list.ForEach(x => Repository.Detach(x));
            list.ForEach(TransformToGet);
            return list;
        }

        /// <summary>
        /// Executa uma solicitação baseada em uma especificação e retorna uma lista paginada de resultados.
        /// </summary>
        /// <param Nome="spec">
        /// A especificação de requisição.
        /// </param>
        /// <param Nome="pagination">
        /// A paginação desejada.
        /// </param>
        /// <returns>
        /// A lista paginada de resultados.
        /// </returns>
        public virtual async Task<List<T>> ListarAsync(IDnSpecification spec, DnPaginacao pagination = null)
        {
            var list = await Repository.ListAsync(spec, pagination);
            list.ForEach(x => Repository.Detach(x));
            list.ForEach(TransformToGet);
            return list;
        }

        /// <summary>
        /// Executa uma solicitação baseada em uma especificação e retorna um resultado ou nulo quando a consulta não é satisfeita.
        /// </summary>
        /// <typeparam Nome="TO">
        /// O tipo de saida desejada. Deve ser o mesmo definido na saida da especificação.
        /// </typeparam>
        /// <param Nome="spec">
        /// A especificação de requisição.
        /// </param>
        /// <returns>
        /// O item referente à consulta ou nulo.
        /// </returns>
        public virtual async Task<TO> FirstOrDefaultSelectAsync<TO>(IDnSpecification<TO> spec)
        {
            var entity = await Repository.FirstOrDefaultSelectAsync(spec);
            entity = Repository.Detach(entity);
            TransformToGet(entity);
            return entity;
        }

        /// <summary>
        /// Executa uma solicitação baseada em uma especificação e retorna um resultado ou nulo quando a consulta não é satisfeita.
        /// </summary>
        /// <param Nome="spec">
        /// A especificação de requisição.
        /// </param>
        /// <returns>
        /// O item referente à consulta ou nulo.
        /// </returns>

        public virtual async Task<T> FirstOrDefaultAsync(IDnSpecification spec)
        {
            var entity = await Repository.FirstOrDefaultAsync(spec);
            entity = Repository.Detach(entity);
            TransformToGet(entity);
            return entity;
        }

        public virtual async Task<T> ObterOUnicoOuPadraoAsync(IDnSpecification spec)
        {
            var entity = await Repository.SingleOrDefaultAsync(spec);
            entity = Repository.Detach(entity);
            TransformToGet(entity);
            return entity;
        }

        /// IsAsNoTracking
        public virtual async Task<TO> SingleOrDefaultSelectAsync<TO>(IDnSpecification<TO> spec)
        {
            var entity = await Repository.SingleOrDefaultSelectAsync(spec);
            entity = Repository.Detach(entity);
            TransformToGet(entity);
            return entity;
        }

        /// <summary>
        /// Retorna a quantidade de itens existentes que satisfaçam a uma especificação
        /// </summary>
        /// <typeparam Nome="TO">
        /// O tipo de saida desejada. Deve ser o mesmo definido na saida da especificação.
        /// </typeparam>
        /// <param Nome="spec">
        /// A especificação de requisição.
        /// </param>
        /// <returns>
        /// A quantidade de itens.
        /// </returns>
        public virtual async Task<int> CountSelectAsync<TO>(IDnSpecification<TO> spec) => await Repository.CountSelectAsync(spec);

        /// <summary>
        /// Retorna a quantidade de itens existentes que satisfaçam a uma especificação
        /// </summary>
        /// <param Nome="spec">
        /// A especificação de requisição.
        /// </param>
        /// <returns>
        /// A quantidade de itens.
        /// </returns>
        public virtual async Task<int> CountAsync(IDnSpecification spec) => await Repository.CountAsync(spec);

        // Todo2 documentar
        public virtual async Task<int> CountAsync() => await Repository.CountAsync();

        // Todo2 documentar
        public virtual void RemoveRange(IDnSpecification spec) => Repository.RemoveRange(spec);

        public virtual async Task TruncateAsync(string APAGAR_TUDO)
        {
            Validation.EliminarTudo(APAGAR_TUDO);
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
        /// <param Nome="entities">
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
        /// <param Nome="entity">
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
            var exists = SessionRequest.ContextDnValidationException.Inconsistencies.RemoveAll(x => x.ExceptionType == nameof(EntityExistsDnValidationException)) > 0;

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
        public virtual async Task<T> BuscaAsync(T entity, bool checkId = true, bool detach = true)
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
        /// <param Nome="entity">
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
        /// <param Nome="entity">
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
        /// Remover um item do banco de dados baseado em seu identificador.
        /// </summary>
        /// <param Nome="entity">
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

            throw new IncorrectDevelopmentException($"You can not initialize the {nameof(DnService<T>)}");
        }


        #endregion
    }
}