// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluent.Architecture.Core.Interfaces;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Services;
using Fluent.Architecture.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Fluent.Architecture.Core.Models
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
            TransactionObjects.Dispose();

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