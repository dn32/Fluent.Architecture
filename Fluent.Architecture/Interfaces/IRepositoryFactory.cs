using Fluente.Arquitetura.Base.Models;
using Fluente.Arquitetura.Services;
namespace Fluente.Arquitetura.Nucleo.Interfaces
{
    internal interface IRepositoryFactory
    {
        IFluenteRepository<T> Create<T>(ITransactionObjects transactionObjects, FluenteService<T> service) where T : EntidadeBase;
    }
}
