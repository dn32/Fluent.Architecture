// ReSharper disable CommentTypo

using System;
using System.Security.Claims;
using System.Web;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Services;
#if NET461
using System.Web.Mvc;
#else
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
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
        protected internal TS Service { get; set; }

        public Guid SessionRequestId => Service.SessionRequestId;

        public ClaimsPrincipal ServiceUser => Service.User;

        public HttpContextBase ServiceHttpContext => Service.LocalHttpContext;

#if NET461

        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            Service = ServiceFactory.Create<TS>(HttpContext);
            base.OnActionExecuting(context);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Service.TransactionObjects.Session.SaveChanges();
            Service.Dispose(true); base.OnActionExecuted(filterContext);
        }
#else
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    if (Service == null)
        //    {
        //        Service = ServiceFactory.Create<TS>(HttpContext);
        //    }

        //    base.OnActionExecuting(context);
        //}
#endif
    }
}
