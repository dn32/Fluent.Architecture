using dn32.infra.dados;
using dn32.infra.Services;
namespace dn32.infra.Nucleo.Interfaces
{
    internal interface IRepositoryFactory
    {
        IDnRepository<T> Create<T>(ITransactionObjects transactionObjects, DnService<T> service) where T : EntidadeBase;
    }
}
