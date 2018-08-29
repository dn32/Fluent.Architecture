#if NET461
using Fluent.Architecture.Test.SupportElements;
using Fluent.Architecture.Test.SupportElements.Mock;
using NUnit.Framework;
using System.Runtime.InteropServices;

namespace Fluent.Architecture.Test.Test
{
    [TestFixture]
    [ComVisible(true)]
    public class ControllerTest : FluentInternalTest
    {
        [Test]
        public void BaseControllerTest()
        {
            UserControllerInstance.SetLocalHttpContext(null);

            Assert.Null(UserControllerInstance.HttpContext);

            var httpContext = MockUtil.GetHttpContext();
            UserControllerInstance.SetLocalHttpContext(httpContext);

            Assert.AreEqual(httpContext, UserControllerInstance.HttpContext);
        }
    }
}
#endif
