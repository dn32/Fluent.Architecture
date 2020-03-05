using dn32.infra.nucleo.excecoes;
using dn32.infra.servicos;
using System.Linq;
using dn32.infra.dados;

namespace dn32.infra.Specifications
{
    public abstract class BaseSpecification
    {
        internal BaseSpecification() { }

        protected bool IgnoreOrder { get; set; } = false;

        public DnServicoTransacionalBase Service { get; set; }

        protected IQueryable<TX> Get<TX>() where TX : EntidadeBase
        {
            if (Service == null)
            {
                throw new DesenvolvimentoIncorretoException($"Failed to initialize specification [{GetType().Name}].\nYou must use [CriarEspecificacao] present in the service or controller.");
            }

            var transactionObjects = Service.ObjetosDaTransacao;
            return transactionObjects.GetObjectQueryInternal<TX>();
        }

        internal void SetService(DnServicoTransacionalBase service)
        {
            Service = service;
        }
    }
}