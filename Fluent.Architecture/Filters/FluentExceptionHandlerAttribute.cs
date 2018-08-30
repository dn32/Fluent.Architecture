// ReSharper disable CommentTypo
#if NET461
using System.Web.Mvc;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Util;
using Fluent.Architecture.Validation;

#else
using System;
using Microsoft.AspNetCore.Mvc.Filters;
#endif

namespace Fluent.Architecture.Filters
{
#if NET461

    /// <inheritdoc />
    public class FluentExceptionHandlerAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.HttpContext.IsCustomErrorEnabled)
            {
                if (filterContext.Exception is ContextFluentValidationException exception)
                {
                    filterContext.Result = new CustomJsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new
                        {
                            exception.Message,
                            ValidationError = true
                        }
                    };
                }
                else
                {
                    filterContext.Result = new CustomJsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new
                        {
                            Error = true,
                            filterContext.Exception.Message
                        }
                    };
                }

                // Todo implementar o log de erros aqui posteriormente
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }
        }
    }
#else

    // public class FluentExceptionHandlerAttribute 

    // {

    // public void OnException(ExceptionContext filterContext)

    // {

    // throw new NotImplementedException();

    // }

    // }
#endif

    // Todo Implementar para net core
}

