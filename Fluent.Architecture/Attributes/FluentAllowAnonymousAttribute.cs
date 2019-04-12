#if !NET461

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