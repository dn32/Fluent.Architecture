#if NET461
using System.Runtime.InteropServices;
using Fluent.Architecture.Sample.Test.SupportElements;
using Fluent.Architecture.Test.Mock;
using NUnit.Framework;

namespace Fluent.Architecture.Sample.Test.Test
{
    [TestFixture]
    [ComVisible(true)]
    public class ControllerTest : FluentInternalTest
    {
        [Test]
        public void BaseControllerTest()
        {
            this.UserControllerInstance.SetLocalHttpContext(null);

            Assert.Null(this.UserControllerInstance.HttpContext);

            var httpContext = MockUtil.GetHttpContext();
            this.UserControllerInstance.SetLocalHttpContext(httpContext);

            Assert.AreEqual(httpContext, this.UserControllerInstance.HttpContext);
        }
    }
}
#endif
