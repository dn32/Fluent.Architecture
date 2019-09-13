using Microsoft.AspNetCore.Http;
using System;

namespace Fluent.Architecture.Test.Mock
{
    public static class MockHttpContextFactory
    {
        public static HttpContext Create(IHeaderDictionary Headers)
        {
#if NETCOREAPP3_0
            throw new NotImplementedException();
#else
            return new MockDefaultHttpContext(Headers);
#endif
        }
    }
}
