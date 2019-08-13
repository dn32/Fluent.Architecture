// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluent.Architecture.Entities;
using Fluent.Architecture.Util;
using Fluent.Architecture.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

public class FluentExceptionHandlerAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext filterContext)
    {
        if (filterContext?.Exception is ContextFluentValidationException exception)
        {
            filterContext.Result = new CustomJsonResult(new ValidationReturn
            {
                Message = exception.Message,
                ValidationError = true
            });
        }
        else
        {
            filterContext.Result = new CustomJsonResult(new
            {
                Error = true,
                filterContext.Exception.Message
            });
        }

        // Todo implementar o log de erros aqui posteriormente
        filterContext.ExceptionHandled = true;
        filterContext.HttpContext.Response.Clear();
        filterContext.HttpContext.Response.StatusCode = 500;
    }
}
