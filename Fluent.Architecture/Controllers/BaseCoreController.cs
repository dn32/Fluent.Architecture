// ReSharper disable CommentTypo
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace Fluent.Architecture.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// O controlador base de referência para todos os controladores do sistema.
    /// </summary>
    public abstract class BaseController : Controller
    {
        private HttpContext _localHttpContext;
        
        public new HttpContext HttpContext => this._localHttpContext ?? base.HttpContext;

        public new IPrincipal User => HttpContext.User;

        protected void SetLocalHttpContext(HttpContext httpContext)
        {
            this._localHttpContext = httpContext;
        }
    }
}
