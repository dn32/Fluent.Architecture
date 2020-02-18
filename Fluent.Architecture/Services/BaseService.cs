// -----------------------------------------------------------------------
// <copyright company="Dn System">
//     Copyright © Dn System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using dn32.infra.Factory;
using dn32.infra.Nucleo.Interfaces;
using dn32.infra.Nucleo.Models;
using dn32.infra.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace dn32.infra.Services
{
    /// <summary>
    /// Serviço base de todos os serviços do sistema.
    /// </summary>
    public abstract class BaseService
    {
        //Temporariamente fora do escopo
        //protected internal virtual Tipo RepositoryType => null;

        protected virtual Type ValidationType => null;

        protected virtual BaseValidation Validation { get; set; }

        protected virtual IBaseRepository Repository { get; set; }

        /// <summary>
        /// Entidade organizadora da injeção de dependência e do contexto da requisição do usuário.
        /// </summary>
        protected internal UserSessionRequest SessionRequest { get; private set; }

        /// <summary>
        /// Obtem o identificador da sessão da requisição atual.
        /// </summary>
        public Guid SessionRequestId => this.SessionRequest.SessionRequestId;

        /// <summary>
        /// HttpContext da requisição vinda do controller.
        /// </summary>

        protected internal HttpContext LocalHttpContext => this.SessionRequest.LocalHttpContext;

        /// <summary>
        /// Usuário do sistema.
        /// </summary>
        protected internal ClaimsPrincipal User => this.SessionRequest.LocalHttpContext.User as ClaimsPrincipal;

        private bool Disposed { get; set; }

        /// <summary>
        /// Obtem a injeção de dependência de propriedades Lazy-loading.
        /// </summary>
        /// <typeparam Nome="TS">
        /// Tipo de serviço.
        /// </typeparam>
        /// <param Nome="sessionId">
        /// Identificador de sessão do usuário para a requisição atual.
        /// </param>
        /// <returns>
        /// O serviço solicitado pela propriedade.
        /// </returns>
        public virtual BaseService GetServiceDependency<TS>(string sessionId) where TS : BaseService, new()
        {
            var sessionIdGuid = Guid.Parse(sessionId);
            this.SessionRequest = Setup.GetUserRequestSession(sessionIdGuid);

            if (this.SessionRequest.Services.TryGetValue(typeof(TS), out var ser))
            {
                return ser as TS;
            }

            var service = ServiceFactory.CreateInternalServiceRuntime(typeof(TS), sessionIdGuid) as TS;
            this.SessionRequest.Services.Add(typeof(TS), service);
            return service;
        }

        public void Dispose(bool primaryService)
        {
            if (this.Disposed)
            {
                return;
            }

            this.Disposed = true;
            this.SessionRequest.Dispose(primaryService);
        }

        /// <summary>
        /// Permite definir a sessão do usuário para a requisição atual.
        /// </summary>
        /// <param Nome="sessionRequest">
        /// A sessão do usuário.
        /// </param>
        protected internal virtual void SetUserSession(UserSessionRequest sessionRequest)
        {
            this.SessionRequest = sessionRequest;
        }
    }
}