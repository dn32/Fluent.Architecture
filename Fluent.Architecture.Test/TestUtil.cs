// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using System;
using System.Runtime.InteropServices;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Test.Mock;
using Fluent.Architecture.Test.Mock.ControllerMock;

namespace Fluent.Architecture.Test
{
    [ComVisible(true)]
    public static class TestUtil
    {
        private static readonly Random Random = new Random();

        public static BaseController GetController(Type controllerType)
        {
            return MockControllerFactory.Create(controllerType);
        }

        public static int NextRandom()
        {
            return Random.Next(1, int.MaxValue);
        }

        public static TR ExecuteAndResult<TR, TC>(TC controller, Func<TC, object> actionMethod) where TC : BaseController
        {
            controller.OnActionExecuting(MockActionExecutingContextFactory.Create(controller));
            var ret = actionMethod(controller);
            controller.OnActionExecuted(MockActionExecutedContextFactory.Create(controller));
            return (TR)ret;
        }

        public static object Execute<TC>(TC controller, Func<TC, object> actionMethod) where TC : BaseController
        {
            controller.OnActionExecuting(MockActionExecutingContextFactory.Create(controller));
            var ret = actionMethod(controller);
            controller.OnActionExecuted(MockActionExecutedContextFactory.Create(controller));
            return ret;
        }
    }
}
