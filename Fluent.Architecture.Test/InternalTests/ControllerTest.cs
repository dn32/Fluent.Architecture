#if NET461
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests;
using Fluent.Architecture.Test.Mock;
using Xunit;

namespace Fluent.Architecture.Test.InternalTests
{
    public class ControllerTest
    {
#if NET461

        [Fact]
        public void BaseControllerTest()
        {
            var controller = TestUtil.GetController(typeof(UserController));
            controller.SetLocalHttpContext(null);

            Assert.Null(controller.HttpContext);

            var httpContext = MockUtil.GetHttpContext();
            controller.SetLocalHttpContext(httpContext);

            Assert.Equal(httpContext, controller.HttpContext);
        }
#endif
    }
}
#endif
