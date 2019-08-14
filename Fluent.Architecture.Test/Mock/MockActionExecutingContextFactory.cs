using Fluent.Architecture.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace Fluent.Architecture.Test.Mock.ControllerMock
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
