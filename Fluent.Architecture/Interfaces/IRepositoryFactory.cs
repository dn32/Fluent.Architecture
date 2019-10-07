using Fluent.Architecture.Services;
using Fluent.Architecture.Core.Models;

namespace Fluent.Architecture.Core.Interfaces
{
    internal interface IRepositoryFactory
    {
        IFluentRepository<T> Create<T>(ITransactionObjects transactionObjects, FluentService<T> service) where T : BaseEntity;
    }
}
