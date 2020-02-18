using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace dn32.infra.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FluenteAllowAnonymousAttribute : ActionFilterAttribute
    {
    }
}
