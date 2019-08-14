// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Fluent.Architecture.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Fluent.Architecture.Test.Mock
{
    public static class MockActionExecutedContextFactory
    {
        public static ActionExecutedContext Create(BaseController controller)
        {
            var actionContext = new ActionContext(
                      new DefaultHttpContext(),
                      new RouteData(),
                      new ActionDescriptor());

            var context = new ActionExecutingContext(
                actionContext,
                filters: new List<IFilterMetadata>(),
                actionArguments: new Dictionary<string, object>(),
                controller: controller);

            return new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), controller);
        }
    }
}