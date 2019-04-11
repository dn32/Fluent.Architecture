// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using System;
using System.Security.Claims;
using System.Web;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Services;
using Fluent.Architecture.Model;
using Fluent.Architecture.Util;

#if NET461
using System.Web.Mvc;

#else
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

#endif

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

        internal protected new TS Service { get; set; }

        protected internal Guid SessionRequestId => Service.SessionRequestId;

        protected internal ClaimsPrincipal ServiceUser => Service.User;

#if NET461
        protected internal HttpContextBase ServiceHttpContext => Service.LocalHttpContext;
#else
        protected internal HttpContext ServiceHttpContext => Service.LocalHttpContext;
#endif

#if NET461
        protected override void OnActionExecuting(ActionExecutingContext context)
#else
        public override void OnActionExecuting(ActionExecutingContext context)
#endif
        {
            Service = ServiceFactory.Create<TS>(HttpContext);
            base.OnActionExecuting(context);
        }

#if NET461
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
#else
        public override void OnActionExecuted(ActionExecutedContext filterContext)
#endif
        {
            var session = Service.TransactionObjects.Session;

            using (var transaction = session.Database.BeginTransaction())
            {
                if (Service.SessionRequest.ContextFluentValidationException.IsValid)
                {
                    session.SaveChanges();

                    if (Service.ExecuteInteractions())
                    {
                        session.SaveChanges();
                    }

                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
            }

            Service.Dispose(true);
            base.OnActionExecuted(filterContext);
        }

        protected internal new JsonResult Json(object data)
        {
#if NET461
            return new CustomJsonResult
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            };
#else
            return new CustomJsonResult(data);
#endif
        }
    }
}
