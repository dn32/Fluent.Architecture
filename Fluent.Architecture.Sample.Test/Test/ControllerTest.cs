//// -----------------------------------------------------------------------
//// <copyright company="Fluent System">
////     Copyright © Fluent System. All rights reserved.
////     TODOS OS DIREITOS RESERVADOS.
//// </copyright>
//// -----------------------------------------------------------------------

//using System.Runtime.InteropServices;
//using Fluent.Architecture.Controllers;
//using Fluent.Architecture.Entities;
//using Fluent.Architecture.Sample.Test.SupportElements;
//using Fluent.Architecture.Test.Mock;
//using NUnit.Framework;

//namespace Fluent.Architecture.Sample.Test.Test
//{
//    [TestFixture]
//    [ComVisible(true)]
//    internal class ControllerTest : FluentInternalTest
//    {
//        [Test]
//        public void BaseControllerTest()
//        {
//            var httpContext = MockHttpContextFactory.Create();
//            UserControllerInstance.SetLocalHttpContext(httpContext);

//            Assert.IsNotNull(UserControllerInstance.HttpContext);
//            Assert.IsNotNull(UserControllerInstance.User);

//            Assert.AreEqual(httpContext, this.UserControllerInstance.HttpContext);
//        }

//        [Test]
//        public void ResultDefaultTest()
//        {
//            var pagination = new FluentPagination(0);
//            var result = new DefaultPaginationTermResult("test", pagination, "new term");

//            Assert.IsNotNull(UserControllerInstance.HttpContext);
//            Assert.IsNotNull(UserControllerInstance.User);

//            Assert.AreEqual("test", result.Data);
//            Assert.AreEqual(pagination, result.Pagination);
//            Assert.AreEqual("new term", result.Term);
//        }
//    }
//}

