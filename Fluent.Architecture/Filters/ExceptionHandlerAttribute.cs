#if NET461
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Fluent.Architecture.Exception;
using Fluent.Architecture.Validation;

#else
using System;
using Microsoft.AspNetCore.Mvc.Filters;
#endif

namespace Fluent.Architecture.Filters
{
#if NET461
    public class ExceptionHandlerAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.HttpContext.IsCustomErrorEnabled)
            {
                if (filterContext.Exception is ContextFluentValidation exception)
                {
                    filterContext.Result = new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = exception
                    };
                }
                else
                {
                    filterContext.Result = new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new
                        {
                            Error = true,
                            filterContext.Exception.Message
                        }
                    };
                }

                //Todo implementar o log de erros aqui posteriormente

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }
        }
    }
#else
    public class ExceptionHandlerAttribute 
    {
        public void OnException(ExceptionContext filterContext)
        {
            throw new NotImplementedException();
        }
    }
#endif
    //Todo Implementar para net core
}

