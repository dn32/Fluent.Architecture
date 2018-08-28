#if NET461
using Fluent.Architecture.Test.SupportElements;
using Fluent.Architecture.Test.SupportElements.Controllers;
using Fluent.Architecture.Test.SupportElements.Mock;
using Fluent.Architecture.Test.TestTools;
using Xunit;

namespace Fluent.Architecture.Test.Test
{
    public class ControllerTest : FluentInternalTest
    {
        [Fact]
        public void BaseControllerTest()
        {
            UserControllerInstance.SetLocalHttpContext(null);

            Assert.Null(UserControllerInstance.HttpContext);

            var httpContext = MockUtil.GetHttpContext();
            UserControllerInstance.SetLocalHttpContext(httpContext);

            Assert.Equal(httpContext, UserControllerInstance.HttpContext);
        }
    }
}
#endif
