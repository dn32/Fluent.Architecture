using dn32.infra.Factory;
using dn32.infra.Nucleo.Interfaces;
using dn32.infra.Nucleo.Models;
using dn32.infra.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace dn32.infra.nucleo.servicos
{
    public abstract class DnServicoBase
    {
        protected virtual BaseValidation Validacao { get; set; }

        protected virtual IBaseRepository Repositorio { get; set; }

        protected internal SessaoDeRequisicaoDoUsuario SessaoDaRequisicao { get; private set; }

        public Guid IdentificadorDaSessaoDaRequisicao => SessaoDaRequisicao.IdentificadorDaSessao;

        protected internal HttpContext HttpContextLocal => SessaoDaRequisicao.LocalHttpContext;

        protected internal ClaimsPrincipal Usuario => SessaoDaRequisicao.LocalHttpContext.User as ClaimsPrincipal;

        private bool Disposed { get; set; }

        public virtual DnServicoBase ObterDependenciaDeServico<TS>(string identificadorDaSessao) where TS : DnServicoBase, new()
        {
            var identificadorDaSessaoGuid = Guid.Parse(identificadorDaSessao);
            SessaoDaRequisicao = Setup.ObterSessaoDeUmaRequisicao(identificadorDaSessaoGuid);

            if (SessaoDaRequisicao.Services.TryGetValue(typeof(TS), out var ser))
            {
                return ser as TS;
            }

            var servico = ServiceFactory.CreateInternalServiceRuntime(typeof(TS), identificadorDaSessaoGuid) as TS;
            SessaoDaRequisicao.Services.Add(typeof(TS), servico);
            return servico;
        }

        public void Dispose(bool servicoPrimario)
        {
            if (Disposed)
            {
                return;
            }

            Disposed = true;
            SessaoDaRequisicao.Dispose(servicoPrimario);
        }

        protected internal virtual void DefinirSessaoDoUsuario(SessaoDeRequisicaoDoUsuario sessaoDaRequisicao)
        {
            SessaoDaRequisicao = sessaoDaRequisicao;
        }
    }
}