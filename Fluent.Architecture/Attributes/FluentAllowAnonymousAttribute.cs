#if NETCOREAPP2_1

using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Fluent.Architecture.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FluentAllowAnonymousAttribute : ActionFilterAttribute
    {
    }
}

#endif