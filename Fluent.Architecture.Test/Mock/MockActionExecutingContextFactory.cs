using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using dn32.infra.nucleo.controladores;

namespace dn32.infra.Test.Mock.ControllerMock
{
    public static class MockActionExecutingContextFactory
    {
        public static ActionExecutingContext Create(DnControladorBase controller)
        {
            var actionContext = MockActionContextFactory.Create();
            return new ActionExecutingContext(
                    actionContext,
                    filters: new List<IFilterMetadata>(),
                    actionArguments: new Dictionary<string, object>(),
                    controller: controller);
        }
    }
}
