using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Fluente.Arquitetura.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FluenteAllowAnonymousAttribute : ActionFilterAttribute
    {
    }
}
