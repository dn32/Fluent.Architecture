using System;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests;
#if NET461
using Fluent.Architecture.Test.Mock.ControllerMock;
using System.Web.Mvc;
using System.Web.Routing;

#endif

namespace Fluent.Architecture.Test.Mock
{
    public static class MockUtil
    {
#if NET461

        public static HttpContextBaseMock GetHttpContext()
        {
            return new HttpContextBaseMock();
        }

        public static BaseController GetMockController(Type controllerType)
        {
            var controller = TestUtil.GetController(controllerType);//typeof(UserController));
            controller.SetLocalHttpContext(new HttpContextBaseMock());
            return controller;
        }

        public static ExceptionContext GetMockExceptionContext(System.Exception exception, BaseController controller, bool customErrorEnabled)
        {
            var controllerContext = GetMockControllerContext(customErrorEnabled);
            return  new ExceptionContext(controllerContext, exception);
        }

        public static ControllerContext GetMockControllerContext(bool customErrorEnabled)
        {
            var controller = TestUtil.GetController(typeof(UserController));
            controller.SetLocalHttpContext(new HttpContextBaseMock(customErrorEnabled));
            return new ControllerContext(controller.HttpContext, new RouteData(), controller);
        }
#endif
    }
}
