// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using dn32.infra.nucleo.controladores;

namespace dn32.infra.Test.Mock
{
    public static class MockActionExecutedContextFactory
    {
        public static ActionExecutedContext Create(DnControladorBase controller)
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