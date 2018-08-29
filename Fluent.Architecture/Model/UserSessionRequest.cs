// ReSharper disable CommentTypo

using System;
using System.Collections.Generic;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Services;
using Fluent.Architecture.Validation;
#if NET461
using System.Web;
#else
using Microsoft.AspNetCore.Http;
#endif

namespace Fluent.Architecture.Model
{
    /// <summary>
    /// Entidade organizadora da injeção de dependência e do contexto da requisição do usuário.
    /// </summary>
    public class UserSessionRequest
    {
        internal Dictionary<Type, BaseService> Services { get; set; }
        internal TransactionObjects TransactionObjects { get; set; }
        internal Guid SessionRequestId { get; set; }
        public ContextFluentValidationException ContextFluentValidationException { get; set; }

        internal object HttpContext;

        public UserSessionRequest()
        {
            this.ContextFluentValidationException = new ContextFluentValidationException();
        }

#if NET461

        /// <summary>
        /// HttpContext da requisição vinda do controller.
        /// </summary>
        public HttpContextBase LocalHttpContext => this.HttpContext as HttpContextBase;
#else

        /// <summary>
        /// HttpContext da requisição vinda do controller.
        /// </summary>
        public HttpContext LocalHttpContext => HttpContext as HttpContext;
#endif
        public void Dispose(bool primaryService)
        {
            Setup.RemoveSession(this.SessionRequestId);
            this.TransactionObjects.Dispose();

            foreach (var service in this.Services.Values)
            {
                service.Dispose(false);
            }

            if (primaryService)
            {
                this.Services.Clear();
            }
        }
    }
}