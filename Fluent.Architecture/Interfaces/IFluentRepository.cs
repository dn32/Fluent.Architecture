using dn32.infra.Interfaces;
using dn32.infra.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dn32.infra.dados;

namespace dn32.infra.Nucleo.Interfaces
{
    public interface IDnRepository<TE> : ITransactionlRepository where TE : EntidadeBase
    {
        #region PROPERTIES

        DnService<TE> Service { get; set; }
        ITransactionObjects TransactionObjects { get; set; }
        Type TransactionObjectsType { get; }

        #endregion

        void RemoveRange(IDnSpecification spec);

        TX Detach<TX>(TX entity);
        Task RemoveRangeAsync(params TE[] entities);
        Task<TE> UpdateAsync(TE entity);
        Task UpdateRangeAsync(IEnumerable<TE> entities);
        Task TruncateAsync();
        Task<TE> RemoveAsync(TE entity);
        Task<bool> ExistsSelectAsync<TO>(ISpec spec);
        Task<bool> ExistsAsync(ISpec spec);
        Task<List<TE>> ListAsync(IDnSpecification spec, DnPaginacao pagination = null);
        Task<List<TO>> ListSelectAsync<TO>(IDnSpecification<TO> spec, DnPaginacao pagination = null);
        Task<TO> FirstOrDefaultSelectAsync<TO>(IDnSpecification<TO> spec);
        Task<TE> FirstOrDefaultAsync(IDnSpecification spec);
        Task<TE> SingleOrDefaultAsync(IDnSpecification spec);
        Task<TO> SingleOrDefaultSelectAsync<TO>(IDnSpecification<TO> spec);
        Task<bool> ExistsAsync(TE entity, bool includeExcludedLogically = false);
        Task<TE> FindAsync(TE entity);
        Task<TE> AddAsync(TE entity);
        Task AddRangeAsync(TE[] entities);
        Task<bool> ExistsOnlyOneAsync(TE entity, bool includeExcludedLogically);
        Task<int> CountSelectAsync<TO>(IDnSpecification<TO> spec);
        Task<int> CountAsync(TE entity, bool includeExcludedLogically);
        Task<int> CountAsync(IDnSpecification spec);
        Task<int> CountAsync();
    }
}
