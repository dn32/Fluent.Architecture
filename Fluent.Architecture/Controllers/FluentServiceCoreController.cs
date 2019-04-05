// ReSharper disable CommentTypo

#if !NET461

using System;
using System.Security.Claims;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Services;
using Fluent.Architecture.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

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
        protected internal TS Service { get; set; }

        protected internal Guid SessionRequestId => Service.SessionRequestId;

        protected internal ClaimsPrincipal ServiceUser => Service.User;

        protected internal HttpContext ServiceHttpContext => Service.LocalHttpContext;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Service = ServiceFactory.Create<TS>(HttpContext);
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var session = Service.TransactionObjects.Session;

            using (var transaction = session.Database.BeginTransaction())
            {
                session.SaveChanges();

                if (Service.ExecuteInteractions())
                {
                    session.SaveChanges();
                }

                transaction.Commit();
            }

            this.Service.Dispose(true);
            base.OnActionExecuted(filterContext);
        }

        protected internal new JsonResult Json(object data)
        {
            return new CustomJsonResult(data);
        }
    }
}

#endif