using Fluent.Architecture.Core.Controllers.ControllerModel;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Interfaces;
using Fluent.Architecture.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Fluent.Architecture.Core.Interfaces
{
    public interface IFluentRepository<TE> : ITransactionlRepository where TE : BaseEntity
    {
        FluentService<TE> Service { get; set; }
        ITransactionObjects TransactionObjects { get; set; }
        Type TransactionObjectsType { get; }
        void RemoveRange(IFluentSpecification spec);
        void Truncate();
        void RemoveRange(TE[] entities);
        TE Remove(TE entity);
        TE Update(TE entity);
        void UpdateRange(TE[] entities);
        List<TO> ListSelect<TO>(IFluentSpecification<TO> spec, FluentPagination pagination = null);
        int Count();
        TO FirstOrDefaultSelect<TO>(IFluentSpecification<TO> spec);
        List<TE> List(IFluentSpecification spec, FluentPagination pagination = null);
        TE FirstOrDefault(IFluentSpecification spec);
        int CountSelect<TO>(IFluentSpecification<TO> spec);
        bool ExistsSelect<TO>(ISpec spec);
        bool Exists(ISpec spec);
        bool Exists(TE entity, bool includeExcludedLogically = false);
        TE Find(TE entity);
        TE Add(TE entity);
        void AddRange(TE[] entities);
        int Count(IFluentSpecification spec);
        TE UpdateAlter(UpdateAlter<TE> value);
    }
}
