
#if !NET461

using Microsoft.AspNetCore.Mvc.Filters;
using Fluent.Architecture.Attributes;
using System;
using System.Linq;
using Fluent.Architecture.Util;

namespace Fluent.Architecture.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AutenticationActionFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionDescriptor.FilterDescriptors.Where(x => x.Filter is FluentAllowAnonymousAttribute).Any())
            {
                return;
            }

            var token = context.HttpContext.Request.Headers["token"];
            context.HttpContext.User = AutenticationUtil.GetPrincipal(token);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}

#endif