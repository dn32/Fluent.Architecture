#if NET461
using Fluent.Architecture.Test.SupportElements.Controllers;
using Fluent.Architecture.Test.SupportElements.Mock;
using Fluent.Architecture.Test.TestTools;
using Xunit;

namespace Fluent.Architecture.Test.Test
{
    public class ControllerTest
    {
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
    }
}
#endif
