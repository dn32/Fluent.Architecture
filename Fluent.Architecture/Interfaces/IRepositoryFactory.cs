using Fluente.Arquitetura.Services;
using Fluente.Arquitetura.Nucleo.Models;

namespace Fluente.Arquitetura.Nucleo.Interfaces
{
    internal interface IRepositoryFactory
    {
        IFluenteRepository<T> Create<T>(ITransactionObjects transactionObjects, FluenteService<T> service) where T : EntidadeBase;
    }
}
