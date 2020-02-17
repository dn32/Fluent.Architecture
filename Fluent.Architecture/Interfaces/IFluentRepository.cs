using Fluente.Arquitetura.Interfaces;
using Fluente.Arquitetura.Nucleo.Models;
using Fluente.Arquitetura.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fluente.Arquitetura.Nucleo.Interfaces
{
    public interface IFluenteRepository<TE> : ITransactionlRepository where TE : EntidadeBase
    {
        #region PROPERTIES

        FluenteService<TE> Service { get; set; }
        ITransactionObjects TransactionObjects { get; set; }
        Type TransactionObjectsType { get; }

        #endregion

        void RemoveRange(IFluenteSpecification spec);

        TX Detach<TX>(TX entity);
        Task RemoveRangeAsync(params TE[] entities);
        Task<TE> UpdateAsync(TE entity);
        Task UpdateRangeAsync(IEnumerable<TE> entities);
        Task TruncateAsync();
        Task<TE> RemoveAsync(TE entity);
        Task<bool> ExistsSelectAsync<TO>(ISpec spec);
        Task<bool> ExistsAsync(ISpec spec);
        Task<List<TE>> ListAsync(IFluenteSpecification spec, FluentePaginacao pagination = null);
        Task<List<TO>> ListSelectAsync<TO>(IFluenteSpecification<TO> spec, FluentePaginacao pagination = null);
        Task<TO> FirstOrDefaultSelectAsync<TO>(IFluenteSpecification<TO> spec);
        Task<TE> FirstOrDefaultAsync(IFluenteSpecification spec);
        Task<TE> SingleOrDefaultAsync(IFluenteSpecification spec);
        Task<TO> SingleOrDefaultSelectAsync<TO>(IFluenteSpecification<TO> spec);
        Task<bool> ExistsAsync(TE entity, bool includeExcludedLogically = false);
        Task<TE> FindAsync(TE entity);
        Task<TE> AddAsync(TE entity);
        Task AddRangeAsync(TE[] entities);
        Task<bool> ExistsOnlyOneAsync(TE entity, bool includeExcludedLogically);
        Task<int> CountSelectAsync<TO>(IFluenteSpecification<TO> spec);
        Task<int> CountAsync(TE entity, bool includeExcludedLogically);
        Task<int> CountAsync(IFluenteSpecification spec);
        Task<int> CountAsync();
    }
}
