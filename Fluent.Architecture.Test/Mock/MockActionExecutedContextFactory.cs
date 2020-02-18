// -----------------------------------------------------------------------
// <copyright company="Dn System">
//     Copyright © Dn System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using dn32.infra.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;

namespace dn32.infra.Test.Mock
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