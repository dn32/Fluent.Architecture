using Microsoft.AspNetCore.Http;

namespace Fluent.Architecture.Test.Mock
{
    public static class MockHttpContextFactory
    {
        public static HttpContext Create()
        {
            return new DefaultHttpContext();
        }
    }
}
