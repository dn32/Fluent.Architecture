// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluent.Architecture.Core.Extensions;
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo(@"Fluent.Architecture.Controller.Test, PublicKey= 00240000048000009400000006020000002400005253413100040000010001006d1cca26da4daf8230bb524d15453c319d38c381589ab07912b8ab6afff8174aad961a74f171790b60e5ed604bc7bad410214a7d59ed6e101c03440e3b1cd055e2bdba377915b076aa15ac9cd6da1acf488a633cb9bc2bb34536b62593950249111ac7c572e02523978ac82d829fe8be29fba6cc4f4e5b668a6cd57d39eee2aa ")]
namespace Fluent.Architecture.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Controlador base para controladores que não possuem entidade. Nesse caso, deve-se informar o serviço a ser usado pelo controlador.
    /// O serviço é inicializado a cada ActionExecuting.
    /// </summary>
    /// <typeparam name="TS">O serviço a ser usado pelo controlador.</typeparam>
    public abstract class FluentServiceController<TS> : BaseController where TS : TransactionalService, new()
    {
        public virtual FluentPagination LastRequestPagination => Service.SessionRequest.Pagination;

        protected internal TS Service { get; set; }

        protected internal Guid SessionRequestId => Service.SessionRequestId;

        protected internal ClaimsPrincipal ServiceUser => Service.User;

        protected internal HttpContext ServiceHttpContext => Service.LocalHttpContext;

        // private IDbContextTransaction Transaction { get; set; }

        internal protected bool TransactionIsStarted { get; set; }

        protected FluentServiceController() 
        {
            Service = null;
        }

        [NonAction]
        protected async Task<DefaultTermResult<T>> ResultAsync<T>(T data, string term)
        {
            await CloseTransactionAsync();
            data = (T)data.FluentResultOrValue();
            return new DefaultTermResult<T>(data, term);
        }

        [NonAction]
        protected async Task<DefaultResult<T>> ResultAsync<T>(T data)
        {
            await CloseTransactionAsync();
            data = (T)data.FluentResultOrValue();
            return new DefaultResult<T>(data);
        }

        [NonAction]
        protected async Task<DefaultPaginationResult<T>> ResultAsync<T>(T data, FluentPagination pagination)
        {
            await CloseTransactionAsync();
            data = (T)data.FluentResultOrValue();
            return new DefaultPaginationResult<T>(data, pagination);
        }

        [NonAction]
        protected async Task<DefaultPaginationTermResult<T>> ResultAsync<T>(T data, FluentPagination pagination, string term)
        {
            await CloseTransactionAsync();
            data = (T)data.FluentResultOrValue();
            return new DefaultPaginationTermResult<T>(data, pagination, term);
        }

        [NonAction]
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            OpenTransaction();
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            CloseTransactionAsync().Wait();
            base.OnActionExecuted(context);
        }

        [NonAction]
        internal protected void OpenTransaction()
        {
            Service = ServiceFactory.Create<TS>(HttpContext);
            //Transaction = Service.TransactionObjects.Session.Database.BeginTransaction();
            TransactionIsStarted = true;
        }

        [NonAction]
        internal protected async Task CloseTransactionAsync()
        {
            if (TransactionIsStarted)
            {
                if (Service.SessionRequest.ContextFluentValidationException.IsValid)
                {
                    if (Service.TransactionObjects != null)
                    {
                        if (Service.TransactionObjects.Session.ChangeTracker.HasChanges())
                        {
                            await Service.TransactionObjects.Session.SaveChangesAsync();
                        }
                    }
                }

                Service.Dispose(true);
            }

            TransactionIsStarted = false;
        }
    }
}
