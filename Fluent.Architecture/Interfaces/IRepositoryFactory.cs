using Fluente.Arquitetura.Services;
using Fluente.Arquitetura.Nucleo.Models;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Base.Models;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Interfaces;
namespace Fluente.Arquitetura.Nucleo.Interfaces
{
    internal interface IRepositoryFactory
    {
        IFluenteRepository<T> Create<T>(ITransactionObjects transactionObjects, FluenteService<T> service) where T : EntidadeBase;
    }
}
