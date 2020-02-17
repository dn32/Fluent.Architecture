using Microsoft.AspNetCore.Routing;

namespace Fluente.Arquitetura.Test.Mock
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
