#if NET461
using System.Web;

namespace Fluent.Architecture.Test.Mock.ControllerMock
{
    public class HttpContextBaseMock : HttpContextBase
    {
        public override HttpResponseBase Response { get; }
        public override HttpRequestBase Request { get; }

        public HttpContextBaseMock(HttpRequestBase request, HttpResponseBase response)
        {
            Request = request;
            Response = response;
        }

        public HttpContextBaseMock()
        {
            Request = new HttpRequestBaseMock();
            Response = new HttpResponseBaseMock();
        }
    }
}
#endif