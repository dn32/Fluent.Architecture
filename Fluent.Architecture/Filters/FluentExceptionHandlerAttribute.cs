// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluent.Architecture.Core.Extensions;
using Fluent.Architecture.Core.Inconsistences;
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Exceptions.ValidationException;
using Fluent.Architecture.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Fluent.Architecture.Filters
{
    public class FluentExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();

            if (filterContext?.Exception?.InnerException is DbUpdateException exception0)
            {
                filterContext.Exception = exception0.InnerException;
            }

            while (filterContext?.Exception is TargetInvocationException exception1)
            {
                filterContext.Exception = exception1.InnerException;
            }

            if (filterContext?.Exception is TargetInvocationException exception2)
            {
                filterContext.Exception = exception2.InnerException;
            }
            
            if (filterContext?.Exception is ContextFluentValidationException exception)
            {
                var inconsistencies = exception.Inconsistencies.Select(inconsistence =>
                {
                    if (inconsistence is FluentUiFieldValidationException field)
                    {
                        return new FluentUiFieldInconsistence
                        {
                            Field = field.Field,
                            Message = field.Message,
                            GlobalizationKey = field.GlobalizationKey,
                            PropertyName = field.PropertyName,
                            FluentException = inconsistence
                        };
                    }
                    else if (inconsistence is FluentPropertyValidationException prop)
                    {
                        return new FluentPropertyInconsistence
                        {
                            Message = prop.Message,
                            GlobalizationKey = prop.GlobalizationKey,
                            PropertyName = prop.PropertyName,
                            FluentException = inconsistence
                        };
                    }
                    else
                    {
                        return new FluentInconsistence
                        {
                            Message = inconsistence.Message,
                            GlobalizationKey = inconsistence.GlobalizationKey,
                            FluentException = inconsistence
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
                    Content = result.ToFluentJson()
                };

                filterContext.Result = content;
                filterContext.HttpContext.Response.StatusCode = 422;
            }
            else
            {
                var stackTrace = new StackTrace(filterContext.Exception, true);
                var frame = stackTrace.GetFrame(0);
                var line = frame.GetFileLineNumber();

                var result = new
                {
                    Error = true,
                    filterContext.Exception.Message,
                    stackTrace,
                    frame,
                    line
                };

                ContentResult content = new ContentResult
                {
                    ContentType = "application/json",
                    Content = result.ToFluentJson()
                };

                filterContext.Result = content;

                filterContext.HttpContext.Response.StatusCode = 500;
            }
        }

        public virtual void GetGlobalization(FluentInconsistence inconsistence)
        {
        }
    }
}