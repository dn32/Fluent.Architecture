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
            var action = context?.ActionDescriptor as ControllerActionDescriptor;
            if (action?.ControllerTypeInfo.GetCustomAttribute<AllowAnonymousAttribute>() != null)
            {
                return;
            }

            if (action?.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>() != null)
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