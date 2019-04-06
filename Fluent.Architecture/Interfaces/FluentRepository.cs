using System.Collections.Generic;
using Fluent.Architecture.Interfaces;
using Fluent.Architecture.Model;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Core.Interfaces
{
    public interface IFluentRepository<TE> : ITransactionlRepository where TE : BaseEntity
    {
        ITransactionObjects TransactionObjects { get; set; }
        FluentService<TE> Service { get; set; }

        void RemoveRange(IFluentSpecification spec);
        void RemoveRange<T>(T[] entities) where T : BaseEntity;
        T Remove<T>(T entity) where T : BaseEntity;
        T Update<T>(T entity) where T : BaseEntity;
        List<TO> ListSelect<TO>(IFluentSpecification<TO> spec, FluentPagination pagination);
        int Count();
        TO FirstOrDefaultSelect<TO>(IFluentSpecification<TO> spec);
        List<BaseEntity> List(IFluentSpecification spec, FluentPagination pagination);
        List<T> List<T>(IFluentSpecification spec, FluentPagination pagination);
        TE FirstOrDefault(IFluentSpecification spec);
        TE FirstOrDefault();
        int CountSelect<TO>(IFluentSpecification<TO> spec);
        bool ExistsSelect<TO>(ISpec spec);
        bool Exists(ISpec spec);
        T Find<T>(T entity) where T : BaseEntity;
        T Add<T>(T entity) where T : BaseEntity;
        void AddRange<T>(T[] entities) where T : BaseEntity;
        int Count(IFluentSpecification spec);
    }
}
