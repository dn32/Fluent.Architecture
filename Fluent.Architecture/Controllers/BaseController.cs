// ReSharper disable CommentTypo

using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Fluent.Architecture.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// O controlador base de referência para todos os controladores do sistema.
    /// </summary>
    public abstract class BaseController : Controller
    {
        private HttpContextBase _localHttpContext;
        
        public new HttpContextBase HttpContext => this._localHttpContext ?? base.HttpContext;

        public new IPrincipal User => HttpContext.User;

        public void SetLocalHttpContext(HttpContextBase httpContext)
        {
            this._localHttpContext = httpContext;
        }
    }
}