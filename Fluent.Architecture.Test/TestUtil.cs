using System;
using System.Linq;
using System.Reflection;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Service;
using Fluent.Architecture.Test.Mock;
#if NET461
using System.Web.Mvc;
using Fluent.Architecture.Test.Mock.ControllerMock;
#else
#endif


namespace Fluent.Architecture.Test
{
    public static class TestUtil
    {
        public static BaseController GetController(Type controllerType)
        {
            return ControllerMockFactory.Create(controllerType);
        }
        
#if NET461
        public static TR Execute<TR>(Type controllerType, string methodName, params object[] parameters) where TR : class
        {
            var controller = TestUtil.GetController(controllerType);
            MethodInfo method;
            if (parameters == null)
            {
                parameters = new object[] { null };
                method = controllerType.GetMethod(methodName);
            }
            else
            {
                var paramTypes = parameters.Select(x => x.GetType()).ToArray();
                method = controllerType.GetMethod(methodName, paramTypes);
            }


            if (method == null)
            {
                throw new System.Exception($"The {methodName} method was not found in {controllerType}.");
            }

            controller.SetLocalHttpContext(new HttpContextBaseMock());

            var fluentOnActionExecuting = controllerType.GetMethod(nameof(FluentServiceController<TransactionalService>.FluentOnActionExecuting));
            if (fluentOnActionExecuting != null)
            {
                fluentOnActionExecuting.Invoke(controller, null);
            }

            try
            {
                var returnObj = method.Invoke(controller, parameters) as JsonResult;
                var fluentOnActionExecuted = controllerType.GetMethod(nameof(FluentServiceController<TransactionalService>.FluentOnActionExecuted));
                if (fluentOnActionExecuted != null)
                {
                    fluentOnActionExecuted.Invoke(controller, null);
                }

                return returnObj?.Data as TR;
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException == null)
                {
                    return ex as TR;
                }

                if (ex.InnerException is FluentValidationException validationError)
                {
                    return validationError as TR;
                }

                throw ex.InnerException;
            }
        }
#endif
    }
}
