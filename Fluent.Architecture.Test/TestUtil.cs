// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Test.Mock;
using Fluent.Architecture.Test.Mock.ControllerMock;
using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;

namespace Fluent.Architecture.Test
{
    [ComVisible(true)]
    public static class TestUtil
    {
        public static BaseController GetController(Type controllerType)
        {
            return MockControllerFactory.Create(controllerType);
        }

        public static TR Execute<TC, TR>(TC controller, Func<TC, object> actionMethod) where TC : BaseController
        {
            controller.OnActionExecuting(MockActionExecutingContextFactory.Create(controller));
            DefaultResult<TR> result;

            try
            {
                result = actionMethod(controller) as DefaultResult<TR>;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            controller.OnActionExecuted(MockActionExecutedContextFactory.Create(controller));
            if(result == null) { return default; }
            return JsonConvert.DeserializeObject<TR>(JsonConvert.SerializeObject(result.Data));
        }

        public static object Execute<TC>(TC controller, Func<TC, object> actionMethod) where TC : BaseController
        {
            controller.OnActionExecuting(MockActionExecutingContextFactory.Create(controller));
            object result;

            try
            {
                result = actionMethod(controller);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            controller.OnActionExecuted(MockActionExecutedContextFactory.Create(controller));
            return result;
        }
    }
}
