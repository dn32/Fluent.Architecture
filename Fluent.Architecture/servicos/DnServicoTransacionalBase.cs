using dn32.infra.Factory;
using dn32.infra.nucleo.servicos;
using dn32.infra.Nucleo.Interfaces;
using dn32.infra.Nucleo.Models;
using dn32.infra.Specifications;
using dn32.infra.Validation;

namespace dn32.infra.servicos
{
    public abstract class DnServicoTransacionalBase : DnServicoBase
    {
        internal ITransactionObjects ObjetosDaTransacao => SessaoDaRequisicao.ObjetosDaTransacao;

        protected new internal TransactionalValidation Validacao
        {
            get => base.Validacao as TransactionalValidation;
            set => base.Validacao = value;
        }

        protected new internal ITransactionlRepository Repositorio
        {
            get => base.Repositorio as ITransactionlRepository;
            set => base.Repositorio = value;
        }

        protected internal override void DefinirSessaoDoUsuario(SessaoDeRequisicaoDoUsuario sessaoDaRequisicao)
        {
            base.DefinirSessaoDoUsuario(sessaoDaRequisicao);
        }

        protected T CriarEspecificacao<T>() where T : BaseSpecification
        {
            return SpecFactory.Criar<T>(this);
        }
    }
}
