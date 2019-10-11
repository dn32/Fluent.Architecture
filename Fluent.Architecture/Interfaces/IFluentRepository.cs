using Fluent.Architecture.Core.Controllers.ControllerModel;
using Fluent.Architecture.Interfaces;
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fluent.Architecture.Core.Interfaces
{
    public interface IFluentRepository<TE> : ITransactionlRepository where TE : BaseEntity
    {
        #region PROPERTIES

        FluentService<TE> Service { get; set; }
        ITransactionObjects TransactionObjects { get; set; }
        Type TransactionObjectsType { get; }

        #endregion

        void RemoveRange(IFluentSpecification spec);
        void RemoveRangeAsync(TE[] entities);


        Task<TE> UpdateAsync(TE entity);
        Task UpdateRangeAsync(IEnumerable<TE> entities);
        Task<TE> UpdateAlterAsync(UpdateAlter<TE> value);
        Task TruncateAsync();
        Task<TE> RemoveAsync(TE entity);
        Task<bool> ExistsSelectAsync<TO>(ISpec spec);
        Task<bool> ExistsAsync(ISpec spec);
        Task<List<TE>> ListAsync(IFluentSpecification spec, FluentPagination pagination = null);
        Task<List<TO>> ListSelectAsync<TO>(IFluentSpecification<TO> spec, FluentPagination pagination = null);
        Task<TO> FirstOrDefaultSelectAsync<TO>(IFluentSpecification<TO> spec);
        Task<TE> FirstOrDefaultAsync(IFluentSpecification spec);
        Task<TE> SingleOrDefaultAsync(IFluentSpecification spec);
        Task<bool> ExistsAsync(TE entity, bool includeExcludedLogically = false);
        Task<TE> FindAsync(TE entity);
        Task<TE> AddAsync(TE entity);
        Task AddRangeAsync(TE[] entities);
        Task<bool> ExistsOnlyOneAsync(TE entity, bool includeExcludedLogically);
        Task<int> CountSelectAsync<TO>(IFluentSpecification<TO> spec);
        Task<int> CountAsync(TE entity, bool includeExcludedLogically);
        Task<int> CountAsync(IFluentSpecification spec);
        Task<int> CountAsync();
    }
}
