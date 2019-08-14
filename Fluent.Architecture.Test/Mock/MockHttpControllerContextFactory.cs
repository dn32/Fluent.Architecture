using Fluent.Architecture.Test.Mock.Novos;
using Microsoft.AspNetCore.Mvc;

namespace Fluent.Architecture.Test.Mock
{
    public static class MockHttpControllerContextFactory
    {
        public static ControllerContext Create()
        {
            var context = new ControllerContext()
            {
                ActionDescriptor = MockControllerActionDescriptorFactory.Create(),
                HttpContext = MockHttpContextFactory.Create(),
                RouteData = MockRouteDataFactory.Create()
            };

            return context;
        }
    }
}
