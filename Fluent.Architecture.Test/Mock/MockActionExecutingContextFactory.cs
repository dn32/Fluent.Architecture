using dn32.infra.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace dn32.infra.Test.Mock.ControllerMock
{
    public static class MockActionExecutingContextFactory
    {
        public static ActionExecutingContext Create(BaseController controller)
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
