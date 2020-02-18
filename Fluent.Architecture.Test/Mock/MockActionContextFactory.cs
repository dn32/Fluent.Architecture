using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace dn32.infra.Test.Mock.ControllerMock
{
    public static class MockActionContextFactory
    {
        public static ActionContext Create()
        {
            return new ActionContext(
                       new DefaultHttpContext(),
                       new RouteData(),
                       new ActionDescriptor());
        }
    }
}
