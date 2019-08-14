using Microsoft.AspNetCore.Routing;

namespace Fluent.Architecture.Test.Mock
{
    public static class MockRouteDataFactory
    {
        public static RouteData Create()
        {
            var route = new RouteData();
            return route;
        }
    }
}
