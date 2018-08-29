#if NET461
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Fluent.Architecture.Test.SupportElements.Mock.ControllerMock
{
    public class HttpContextBaseMock : HttpContextBase
    {
        public override HttpResponseBase Response { get; }

        public override HttpRequestBase Request { get; }

        private bool SetIsCustomErrorEnabled { get; }

        public override IPrincipal User { get; set; }

        public override bool IsCustomErrorEnabled => this.SetIsCustomErrorEnabled;

        public HttpContextBaseMock(HttpRequestBase request, HttpResponseBase response)
        {
            this.Request = request;
            this.Response = response;
            this.User = new ClaimsPrincipal();
        }

        public HttpContextBaseMock()
        {
            this.Request = new HttpRequestBaseMock();
            this.Response = new HttpResponseBaseMock();
            this.User = new ClaimsPrincipal();
        }

        public HttpContextBaseMock(bool isCustomErrorEnabled)
        {
            this.Request = new HttpRequestBaseMock();
            this.Response = new HttpResponseBaseMock();
            this.SetIsCustomErrorEnabled = isCustomErrorEnabled;
            this.User = new ClaimsPrincipal();
        }
    }
}
#endif