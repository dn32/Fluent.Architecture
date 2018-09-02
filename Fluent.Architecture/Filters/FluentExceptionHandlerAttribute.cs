// ReSharper disable CommentTypo
#if NET461
using System.Web.Mvc;
using Fluent.Architecture.Model;
using Fluent.Architecture.Util;
using Fluent.Architecture.Validation;

namespace Fluent.Architecture.Filters
{
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
                        Data = new ValidationReturn
                        {
                            Message= exception.Message,
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
}
#endif

