// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;
using Fluent.Architecture.Sample.Test.SupportElements;
using Fluent.Architecture.Test.Mock;
using NUnit.Framework;

namespace Fluent.Architecture.Sample.Test.Test
{
    [TestFixture]
    [ComVisible(true)]
    internal class ControllerTest : FluentInternalTest
    {
        [Test]
        public void BaseControllerTest()
        {
            var httpContext = MockHttpContextFactory.Create();
            UserControllerInstance.SetLocalHttpContext(httpContext);
            Assert.AreEqual(httpContext, this.UserControllerInstance.HttpContext);
        }
    }
}

