// ReSharper disable CommentTypo

using System;
using System.Security.Claims;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Model;

#if NET461
using System.Web;
#else
using Microsoft.AspNetCore.Http;
#endif

namespace Fluent.Architecture.Service
{
    /// <summary>
    /// Serviço base de todos os serviços do sistema.
    /// </summary>
    public abstract class BaseService
    {
        private bool Disposed { get; set; }

        /// <summary>
        /// Entidade organizadora da injeção de dependência e do contexto da requisição do usuário.
        /// </summary>
        public UserSessionRequest SessionRequest { get; private set; }

        /// <summary>
        /// Obtem o identificador da sessão da requisição atual.
        /// </summary>
        public Guid SessionRequestId => SessionRequest.SessionRequestId;

#if NET461
        /// <summary>
        /// HttpContext da requisição vinda do controller.
        /// </summary>
        public HttpContextBase LocalHttpContext => SessionRequest.LocalHttpContext;
#else
/// <summary>
/// HttpContext da requisição vinda do controller.
/// </summary>
        public HttpContext LocalHttpContext=> SessionRequest.LocalHttpContext;
#endif

        /// <summary>
        /// Usuário do sistema.
        /// </summary>
        protected ClaimsPrincipal User => SessionRequest.LocalHttpContext.User as ClaimsPrincipal;

        /// <summary>
        /// Obtem a injeção de dependência de propriedades Lazy-loading.
        /// </summary>
        /// <typeparam name="TS">
        /// Tipo de serviço.
        /// </typeparam>
        /// <param name="sessionId">
        /// Identificador de sessão do usuário para a requisição atual.
        /// </param>
        /// <returns>
        /// O serviço solicitado pela propriedade.
        /// </returns>
        public virtual BaseService GetServiceDependency<TS>(string sessionId) where TS : BaseService, new()
        {
            var sessionIdGuid = Guid.Parse(sessionId);
            SessionRequest = Setup.GetUserRequestSession(sessionIdGuid);

            if (SessionRequest.Services.TryGetValue(typeof(TS), out var ser))
            {
                return ser as TS;
            }

            var service = ServiceFactory.CreateInternalServiceRuntime(typeof(TS), SessionRequest.TransactionObjects, SessionRequest.LocalHttpContext, sessionIdGuid) as TS;
            SessionRequest.Services.Add(typeof(TS), service);
            return service;
        }

        /// <summary>
        /// Permite definir a sessão do usuário para a requisição atual.
        /// </summary>
        /// <param name="sessionRequest">
        /// A sessão do usuário.
        /// </param>
     protected   internal virtual void SetUserSession(UserSessionRequest sessionRequest)
        {
            SessionRequest = sessionRequest;
        }

        public void Dispose(bool primaryService)
        {
            if (Disposed)
            {
                return;
            }

            Disposed = true;
            SessionRequest.Dispose(primaryService);
        }
    }
}