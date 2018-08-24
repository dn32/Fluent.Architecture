#if NET461
using System.Web;

namespace Fluent.Architecture.Test.Mock.ControllerMock
{
    public class HttpResponseBaseMock : HttpResponseBase
    {
        public override void Clear()
        {
        }

        public override int StatusCode { get; set; }

        public override bool TrySkipIisCustomErrors { get; set; }
    }
}

#endif