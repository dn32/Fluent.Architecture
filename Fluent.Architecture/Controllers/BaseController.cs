// ReSharper disable CommentTypo
#if NET461
using System.Web;
using System.Web.Mvc;
using Fluent.Architecture.Services;

#else
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
#endif

namespace Fluent.Architecture.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// O controlador base de referência para todos os controladores do sistema.
    /// </summary>
    public abstract class BaseController : Controller
    {
#if NET461
        private HttpContextBase localHttpContext;
        
        public new HttpContextBase HttpContext => localHttpContext ?? base.HttpContext;

        public void SetLocalHttpContext(HttpContextBase httpContext)
        {
            localHttpContext = httpContext;
        }
#else
        private HttpContext localHttpContext;

        public new HttpContext HttpContext => localHttpContext ?? base.HttpContext;

        public void SetLocalHttpContext(HttpContext httpContext)
        {
            localHttpContext = httpContext;
        }
#endif
    }
}