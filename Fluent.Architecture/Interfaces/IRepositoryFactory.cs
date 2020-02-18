using dn32.infra.dados;
using dn32.infra.Services;
namespace dn32.infra.Nucleo.Interfaces
{
    internal interface IRepositoryFactory
    {
        IFluenteRepository<T> Create<T>(ITransactionObjects transactionObjects, FluenteService<T> service) where T : EntidadeBase;
    }
}
