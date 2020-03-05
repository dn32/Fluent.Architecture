using dn32.infra.Interfaces;
using dn32.infra.servicos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dn32.infra.dados;

namespace dn32.infra.Nucleo.Interfaces
{
    public interface IDnRepository<TE> : ITransactionlRepository where TE : EntidadeBase
    {
        #region PROPERTIES

        DnServico<TE> Service { get; set; }
        ITransactionObjects TransactionObjects { get; set; }
        Type TransactionObjectsType { get; }

        #endregion

        void RemoverLista(IDnSpecification spec);

        TX Desanexar<TX>(TX entity);
        Task RemoverListaAsync(params TE[] entities);
        Task<TE> AtualizarAsync(TE entity);
        Task AtualizarListaAsync(IEnumerable<TE> entities);
        Task EliminarTudoAsync();
        Task<TE> RemoverAsync(TE entity);
        Task<bool> ExisteAlternativoAsync<TO>(ISpec spec);
        Task<bool> ExisteAsync(ISpec spec);
        Task<List<TE>> ListAsync(IDnSpecification spec, DnPaginacao pagination = null);
        Task<List<TO>> ListarAlternativoAsync<TO>(IDnSpecification<TO> spec, DnPaginacao pagination = null);
        Task<TO> PrimeiroOuPadraoAlternativoAsync<TO>(IDnSpecification<TO> spec);
        Task<TE> PrimeiroOuPadraoAsync(IDnSpecification spec);
        Task<TE> SingleOrDefaultAsync(IDnSpecification spec);
        Task<TO> UnicoOuPadraoAlternativoAsync<TO>(IDnSpecification<TO> spec);
        Task<bool> ExisteAsync(TE entity, bool includeExcludedLogically = false);
        Task<TE> FindAsync(TE entity);
        Task<TE> AdicionarAsync(TE entity);
        Task AdicionarListaAsync(TE[] entities);
        Task<bool> HaSomenteUmAsync(TE entity, bool includeExcludedLogically);
        Task<int> QuantidadeAlternativoAsync<TO>(IDnSpecification<TO> spec);
        Task<int> QuantidadeAsync(TE entity, bool includeExcludedLogically);
        Task<int> QuantidadeAsync(IDnSpecification spec);
        Task<int> QuantidadeTotalAsync();
    }
}
