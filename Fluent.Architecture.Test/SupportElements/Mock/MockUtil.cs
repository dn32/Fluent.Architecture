using System;
using System.Web.Mvc;
using System.Web.Routing;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Test.SupportElements.Controllers;
using Fluent.Architecture.Test.SupportElements.Mock.ControllerMock;
using Fluent.Architecture.Test.TestTools;

#if NET461

#endif

namespace Fluent.Architecture.Test.SupportElements.Mock
{
    public static class MockUtil
    {
#if NET461

        public static HttpContextBaseMock GetHttpContext()
        {
            return new HttpContextBaseMock();
        }

        public static TC GetMockController<TC>() where TC : class
        {
            return GetMockController(typeof(TC)) as TC;
        }

        public static BaseController GetMockController(Type controllerType)
        {
            var controller = TestUtil.GetController(controllerType);//typeof(UserController));
            controller.SetLocalHttpContext(new HttpContextBaseMock());
            return controller;
        }

        public static ExceptionContext GetMockExceptionContext<TC>(System.Exception exception, BaseController controller, bool customErrorEnabled)
        {
            var controllerContext = GetMockControllerContext<TC>(customErrorEnabled);
            return  new ExceptionContext(controllerContext, exception);
        }

        public static ControllerContext GetMockControllerContext<TC>(bool customErrorEnabled)
        {
            var controller = TestUtil.GetController(typeof(TC));
            controller.SetLocalHttpContext(new HttpContextBaseMock(customErrorEnabled));
            return new ControllerContext(controller.HttpContext, new RouteData(), controller);
        }

        public static ControllerContext GetMockControllerContext(BaseController controller)
        {
            controller.SetLocalHttpContext(new HttpContextBaseMock(false));
            return new ControllerContext(controller.HttpContext, new RouteData(), controller);
        }
#endif
    }
}
