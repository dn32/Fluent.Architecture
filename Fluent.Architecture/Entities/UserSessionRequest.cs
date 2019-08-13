// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Fluent.Architecture.Services;
using Fluent.Architecture.Validation;
using Fluent.Architecture.Core.Interfaces;

namespace Fluent.Architecture.Entities
{
    /// <summary>
    /// Entidade organizadora da injeção de dependência e do contexto da requisição do usuário.
    /// </summary>
    public class UserSessionRequest
    {
        internal Dictionary<Type, BaseService> Services { get; set; }
        internal ITransactionObjects TransactionObjects { get; set; }
        internal Guid SessionRequestId { get; set; }
        public ContextFluentValidationException ContextFluentValidationException { get; set; }
        public FluentPagination Pagination { get; set; }

        internal object HttpContext;

        public UserSessionRequest()
        {
            this.ContextFluentValidationException = new ContextFluentValidationException();
        }

        /// <summary>
        /// HttpContext da requisição vinda do controller.
        /// </summary>

        public HttpContext LocalHttpContext => this.HttpContext as HttpContext;

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