// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Security.Claims;
using System.Web.Mvc;
using System.Web.Routing;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Test.Mock.ControllerMock;

namespace Fluent.Architecture.Test.Mock
{
    public static class MockUtil
    {
        //Todo - Restaurar o mock quando possível
        public static HttpContextBaseMock GetHttpContext()
        {
            return new HttpContextBaseMock();
        }

        public static TC GetMockController<TC>(ClaimsPrincipal user = null) where TC : class
        {
            return GetMockController(typeof(TC), user) as TC;
        }

        public static BaseController GetMockController(Type controllerType, ClaimsPrincipal user = null)
        {
            throw new NotImplementedException();
            //var controller = TestUtil.GetController(controllerType);// typeof(UserController));
            //controller.SetLocalHttpContext(new HttpContextBaseMock(user));
            //return controller;
        }

        public static ExceptionContext GetMockExceptionContext<TC>(Exception exception, BaseController controller, bool customErrorEnabled)
        {
            throw new NotImplementedException();
            //var controllerContext = GetMockControllerContext<TC>(customErrorEnabled);
            //return new ExceptionContext(controllerContext, exception);
        }

        public static ControllerContext GetMockControllerContext<TC>(bool customErrorEnabled)
        {
            throw new NotImplementedException();
            //var controller = TestUtil.GetController(typeof(TC));
            //controller.SetLocalHttpContext(new HttpContextBaseMock(customErrorEnabled));
            //return new ControllerContext(controller.HttpContext, new RouteData(), controller);
        }

        public static ControllerContext GetMockControllerContext(BaseController controller)
        {
            throw new NotImplementedException();
            //if (controller.HttpContext == null)
            //{
            //    controller.SetLocalHttpContext(new HttpContextBaseMock(false));
            //}

            //return new ControllerContext(controller.HttpContext, new RouteData(), controller);
        }
    }
}

