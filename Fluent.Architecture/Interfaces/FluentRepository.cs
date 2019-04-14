using System;
using System.Collections.Generic;
using Fluent.Architecture.Interfaces;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Core.Interfaces
{
    public interface IFluentRepository<TE> : ITransactionlRepository where TE : BaseEntity
    {
        FluentService<TE> Service { get; set; }
        ITransactionObjects TransactionObjects { get; set; }
        Type TransactionObjectsType { get; }
        void RemoveRange(IFluentSpecification spec);
        void RemoveRange(TE[] entities);
        TE Remove(TE entity);
        TE Update(TE entity);
        List<TO> ListSelect<TO>(IFluentSpecification<TO> spec, FluentPagination pagination = null);
        int Count();
        TO FirstOrDefaultSelect<TO>(IFluentSpecification<TO> spec);
        List<TE> List(IFluentSpecification spec, FluentPagination pagination = null);
        TE FirstOrDefault(IFluentSpecification spec);
        TE FirstOrDefault();
        int CountSelect<TO>(IFluentSpecification<TO> spec);
        bool ExistsSelect<TO>(ISpec spec);
        bool Exists(ISpec spec);
        TE Find(TE entity);
        TE Add(TE entity);
        void AddRange(TE[] entities);
        int Count(IFluentSpecification spec);
    }
}
