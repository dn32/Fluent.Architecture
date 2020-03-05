using dn32.infra.dados;
using dn32.infra.servicos;
namespace dn32.infra.Nucleo.Interfaces
{
    internal interface IRepositoryFactory
    {
        IDnRepository<T> Create<T>(ITransactionObjects transactionObjects, DnServico<T> service) where T : EntidadeBase;
    }
}
