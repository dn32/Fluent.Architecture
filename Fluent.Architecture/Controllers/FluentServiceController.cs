// ReSharper disable CommentTypo
using Fluent.Architecture.Factory;
using Fluent.Architecture.Service;

#if NET461
using System.Web.Routing;
using System.Web.Mvc;
using System.Web;
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
        protected TS Service { get; private set; }

#if NET461
        public virtual void FluentOnActionExecuting()
        {
            Service = ServiceFactory.Create<TS>(HttpContext);
        }

        public virtual void FluentOnActionExecuted()
        {
            Service.TransactionObjects.Session.SaveChanges();
            Service.Dispose(true);
        }

        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            FluentOnActionExecuting();
            base.OnActionExecuting(context);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            FluentOnActionExecuted();
            base.OnActionExecuted(filterContext);
        }
#else
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (Service == null)
            {
                Service = ServiceFactory.Create<TS>(HttpContext);
            }

            base.OnActionExecuting(context);
        }
#endif
    }
}
