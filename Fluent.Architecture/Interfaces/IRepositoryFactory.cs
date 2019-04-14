using Fluent.Architecture.Entities;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Core.Interfaces
{
    internal interface IRepositoryFactory
    {
        IFluentRepository<T> Create<T>(ITransactionObjects transactionObjects, FluentService<T> service) where T : BaseEntity;
    }
}
