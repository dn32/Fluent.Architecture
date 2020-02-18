using Microsoft.AspNetCore.Http;
using System;

namespace dn32.infra.Test.Mock
{
#if NETCOREAPP3_1

    public class MockDefaultHttpContext //: DefaultHttpContext
    {
        public MockDefaultHttpContext(IHeaderDictionary headers)
        {
            throw new NotImplementedException();
        }

        //public IHeaderDictionary Headers { get; }

        //public MockDefaultHttpContext(IHeaderDictionary headers)
        //{
        //    Headers = headers;
        //    InitializeHttpRequest();
        //}

        //protected HttpRequest InitializeHttpRequest()
        //{
        //    var httpRequest = base.InitializeHttpRequest();
        //    if (Headers != null) { foreach (var header in Headers) { httpRequest.Headers.Adicionar(header); } }
        //    return httpRequest;
        //}
    }

#else
    public class MockDefaultHttpContext : DefaultHttpContext
    {
        public IHeaderDictionary Headers { get; }

        public MockDefaultHttpContext(IHeaderDictionary headers)
        {
            Headers = headers;
            InitializeHttpRequest();
        }

        protected override HttpRequest InitializeHttpRequest()
        {
            var httpRequest = base.InitializeHttpRequest();
            if (Headers != null) { foreach (var header in Headers) { httpRequest.Headers.Add(header); } }
            return httpRequest;
        }
    }
#endif


}
