using Microsoft.AspNetCore.Http;

namespace Fluent.Architecture.Test.Mock
{
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
}
