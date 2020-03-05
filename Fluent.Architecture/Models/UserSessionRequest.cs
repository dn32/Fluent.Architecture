using dn32.infra.extensoes;
using dn32.infra.Nucleo.Interfaces;
using dn32.infra.servicos;
using dn32.infra.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using dn32.infra.dados;
using dn32.infra.nucleo.servicos;

namespace dn32.infra.Nucleo.Models
{
    /// <summary>
    /// Entidade organizadora da injeção de dependência e do contexto da requisição do usuário.
    /// </summary>
    public class SessaoDeRequisicaoDoUsuario
    {
        internal Dictionary<Type, DnServicoBase> Services { get; set; }
        internal ITransactionObjects ObjetosDaTransacao { get; set; }
        internal Guid IdentificadorDaSessao { get; set; }
        public ContextDnValidationException ContextDnValidationException { get; set; }
        public DnPaginacao Pagination { get; set; }

        internal object HttpContext;

        public SessaoDeRequisicaoDoUsuario()
        {
            this.ContextDnValidationException = new ContextDnValidationException();
        }

        /// <summary>
        /// HttpContext da requisição vinda do controller.
        /// </summary>

        public HttpContext LocalHttpContext => this.HttpContext as HttpContext;

        public void Dispose(bool primaryService)
        {
            Setup.RemoverSessaoDeRequisicao(this.IdentificadorDaSessao);
            ObjetosDaTransacao?.Dispose();

            foreach (var service in this.Services.Values)
            {
                service.Dispose(false);
            }

            if (primaryService)
            {
                Services.Clear();
            }
        }

        public Dictionary<string, List<object>> CodeAvailableForEntity { get; set; } = new Dictionary<string, List<object>>();

        internal object GetCodeAvailableForEntity(string key)
        {
            lock (CodeAvailableForEntity)
            {
                if (CodeAvailableForEntity.TryGetValue(key, out List<object> list))
                {
                    return list.Next();
                }
                else
                {
                    return null;
                }
            }
        }

        internal void SetCodeAvailableForEntity(string key, List<object> entitiCodes)
        {
            lock (CodeAvailableForEntity)
            {
                CodeAvailableForEntity.Remove(key);
                CodeAvailableForEntity.Add(key, entitiCodes);
            }
        }
    }
}