using dn32.infra.Exceptions.ValidationException;
using dn32.infra.Nucleo.Inconsistences;
using dn32.infra.Nucleo.Models;
using dn32.infra.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using dn32.infra.extensoes;

namespace dn32.infra.Filters
{
    public class FluenteExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext?.Exception == null) { return; }

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();

            if (filterContext.Exception?.InnerException is DbUpdateException exception0)
            {
                filterContext.Exception = exception0.InnerException;
            }

            while (filterContext.Exception is TargetInvocationException exception1)
            {
                filterContext.Exception = exception1.InnerException;
            }

            if (filterContext.Exception is TargetInvocationException exception2)
            {
                filterContext.Exception = exception2.InnerException;
            }

            if (filterContext.Exception is ContextFluenteValidationException exception)
            {
                var inconsistencies = exception.Inconsistencies.Select(inconsistence =>
                {
                    if (inconsistence is FluenteUiFieldValidationException field)
                    {
                        return new FluenteUiFieldInconsistence
                        {
                            Field = field.Field,
                            Message = field.Message,
                            GlobalizationKey = field.GlobalizationKey,
                            PropertyName = field.PropertyName,
                            FluenteException = inconsistence
                        };
                    }
                    else if (inconsistence is FluentePropertyValidationException prop)
                    {
                        return new FluentePropertyInconsistence
                        {
                            Message = prop.Message,
                            GlobalizationKey = prop.GlobalizationKey,
                            PropertyName = prop.PropertyName,
                            FluenteException = inconsistence
                        };
                    }
                    else
                    {
                        return new FluenteInconsistence
                        {
                            Message = inconsistence.Message,
                            GlobalizationKey = inconsistence.GlobalizationKey,
                            FluenteException = inconsistence
                        };
                    }
                })
                .ToList();

                inconsistencies.ForEach(GetGlobalization);

                // filterContext.Result =
                var result = new ValidationReturn
                {
                    Inconsistencies = inconsistencies,
                    Message = exception.Message,
                    ValidationError = true
                };

                ContentResult content = new ContentResult
                {
                    ContentType = "application/json",
                    Content = result.SerializarParaFluenteJson()
                };

                filterContext.Result = content;
                filterContext.HttpContext.Response.StatusCode = 422;
            }
            else
            {
                if (filterContext.Exception == null) { return; }
                var stackTrace = new StackTrace(filterContext.Exception, true);
                var frame = stackTrace.GetFrame(0);
                var line = frame?.GetFileLineNumber() ?? -1;

                var result = new
                {
                    Error = true,
                    filterContext.Exception?.Message,
                    stackTrace,
                    line
                };

                ContentResult content = new ContentResult
                {
                    ContentType = "application/json",
                    Content = result.SerializarParaFluenteJson()
                };

                filterContext.Result = content;
                filterContext.HttpContext.Response.StatusCode = 500;
            }
        }

        public virtual void GetGlobalization(FluenteInconsistence inconsistence)
        {
        }
    }
}