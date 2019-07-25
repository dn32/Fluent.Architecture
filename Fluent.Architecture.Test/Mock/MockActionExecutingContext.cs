// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fluent.Architecture.Controllers;

namespace Fluent.Architecture.Test.Mock
{
    public static class MockActionExecutingContext
    {
        public static ActionExecutingContext Create(BaseController controller, string actionName)
        {
            throw new NotImplementedException();
            //var controllerContext = MockUtil.GetMockControllerContext(controller);
            //var controllerDescriptor = new MockControllerDescriptor(controller.GetType());
            //var actionDescriptor = new MockActionDescriptor(actionName, controllerDescriptor);
            //var actionParameters = new Dictionary<string, object>();

            //return new ActionExecutingContext(controllerContext, actionDescriptor, actionParameters);
        }
    }
}