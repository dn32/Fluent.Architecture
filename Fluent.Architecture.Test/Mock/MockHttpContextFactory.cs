using Microsoft.AspNetCore.Http;
using System;

namespace dn32.infra.Test.Mock
{
    public static class MockHttpContextFactory
    {
        public static HttpContext Create(IHeaderDictionary Headers)
        {
#if NETCOREAPP3_1
            throw new NotImplementedException();
#else
            return new MockDefaultHttpContext(Headers);
#endif
        }
    }
}
