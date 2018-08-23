#if NET461
using System;
using System.Collections.Specialized;
using System.Web;

namespace Fluent.Architecture.Test.Mock.ControllerMock
{
    public sealed class HttpRequestBaseMock : HttpRequestBase
    {
        public override Uri Url { get; }

        public override string ApplicationPath { get; }

        public override NameValueCollection ServerVariables { get; }

        public HttpRequestBaseMock()
        {
            Url = new Uri("http://localhost");
            ApplicationPath = "/";//AppDomain.CurrentDomain.BaseDirectory;

            ServerVariables = new NameValueCollection
                {
                    { "HTTP_HOST", "localhost" }
                };
        }
    }
}
#endif