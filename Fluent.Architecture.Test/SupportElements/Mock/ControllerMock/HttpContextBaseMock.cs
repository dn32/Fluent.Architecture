#if NET461
using System.Web;

namespace Fluent.Architecture.Test.SupportElements.Mock.ControllerMock
{
    public class HttpContextBaseMock : HttpContextBase
    {
        public override HttpResponseBase Response { get; }

        public override HttpRequestBase Request { get; }

        private bool SetIsCustomErrorEnabled { get; set; }

        public override bool IsCustomErrorEnabled => SetIsCustomErrorEnabled;

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

        public HttpContextBaseMock(bool isCustomErrorEnabled)
        {
            Request = new HttpRequestBaseMock();
            Response = new HttpResponseBaseMock();
            SetIsCustomErrorEnabled = isCustomErrorEnabled;
        }
    }
}
#endif