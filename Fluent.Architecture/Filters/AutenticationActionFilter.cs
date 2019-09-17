using Fluent.Architecture.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace Fluent.Architecture.Filters
{
    public class FluentAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var action = context.ActionDescriptor as ControllerActionDescriptor;
            if (action?.ControllerTypeInfo.GetCustomAttributeAny<AllowAnonymousAttribute>() == true)
            {
                return;
            }

            if (action?.MethodInfo.GetCustomAttributeAny<AllowAnonymousAttribute>() == true)
            {
                return;
            }

            OnFluentAuthorizationFilter(context);
        }

        public virtual void OnFluentAuthorizationFilter(AuthorizationFilterContext context)
        {
        }
    }
}