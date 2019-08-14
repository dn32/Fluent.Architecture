using Fluent.Architecture.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Fluent.Architecture.Test.Mock
{
    public static class MockUtil
    {
        public static TC GetMockController<TC>(ClaimsPrincipal user = null) where TC : class
        {
            return GetMockController(typeof(TC), user) as TC;
        }

        public static BaseController GetMockController(Type controllerType, ClaimsPrincipal user = null)
        {
            var controller = TestUtil.GetController(controllerType);// typeof(UserController));
            var context = MockHttpControllerContextFactory.Create();
            controller.ControllerContext = context;
            controller.SetLocalHttpContext(controller.HttpContext);
            return controller;
        }

        public static ExceptionContext GetMockExceptionContext<TC>(Exception exception, BaseController controller)
        {
            var context = MockHttpControllerContextFactory.Create();
            return new ExceptionContext(context, new List<IFilterMetadata>()) { Exception = exception };
        }
    }
}

