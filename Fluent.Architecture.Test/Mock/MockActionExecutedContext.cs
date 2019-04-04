// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System.Web.Mvc;
using Fluent.Architecture.Controllers;

namespace Fluent.Architecture.Test.Mock
{
    public static class MockActionExecutedContext
    {
        public static ActionExecutedContext Create(BaseController controller, string actionName)
        {
            var controllerContext = MockUtil.GetMockControllerContext(controller);
            var controllerDescriptor = new MockControllerDescriptor(controller.GetType());
            var actionDescriptor = new MockActionDescriptor(actionName, controllerDescriptor);

            return new ActionExecutedContext(controllerContext, actionDescriptor, false, null);
        }
    }
}