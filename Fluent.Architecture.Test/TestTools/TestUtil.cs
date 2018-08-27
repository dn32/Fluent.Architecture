// ReSharper disable CommentTypo

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Exception.ValidationException;
using Fluent.Architecture.Services;
using Fluent.Architecture.Test.SupportElements.Controllers;
using Fluent.Architecture.Test.SupportElements.Mock;
using Fluent.Architecture.Test.SupportElements.Mock.ControllerMock;
using Fluent.Architecture.Validation;
using Xunit;

namespace Fluent.Architecture.Test.TestTools
{
    public static class TestUtil
    {
        private static readonly Random Random = new Random();

        private static readonly object SyncLock = new object();

        public static BaseController GetController(Type controllerType)
        {
            return ControllerMockFactory.Create(controllerType);
        }

        public static int NextRandom()
        {
            lock (SyncLock)
            {
                return Random.Next(1, int.MaxValue);
            }
        }

        public static TR Execute<TR>(BaseController controller, string methodName, object parameter)
        {
            return Execute<TR>(controller, methodName, new object[] { parameter });
        }

        public static TR Execute<TR>(BaseController controller, string methodName, object[] parameters, Func<BaseController, TR> action = null)
        {
            var controllerType = controller.GetType();
            MethodInfo method = null;

            if (action == null)
            {
                if (parameters == null || (parameters.Length == 1 && parameters.First() == null))
                {
                    method = controllerType.GetMethod(methodName);
                }
                else
                {
                    var parameterTypes = (from parameter in parameters
                        select parameter == null ? typeof(object) : parameter.GetType()).ToList();
                    method = controllerType.GetMethod(methodName, parameterTypes.ToArray());
                }

                if (method == null)
                {
                    throw new System.Exception($"The {methodName} method was not found in {controllerType}.");
                }
            }

            controller.SetLocalHttpContext(new HttpContextBaseMock());

            var actionExecuting = controllerType.GetMethod("OnActionExecuting", BindingFlags.NonPublic | BindingFlags.Instance);
            if (actionExecuting != null)
            {
                var actionExecutingContext = MockActionExecutingContext.Create(controller, methodName);
                actionExecuting.Invoke(controller, new object[] { actionExecutingContext });
            }

            try
            {
                JsonResult returnObj;
                if (action == null)
                {
                    returnObj = method.Invoke(controller, parameters) as JsonResult;
                }
                else
                {
                    returnObj = new JsonResult{Data = action(controller) };
                }

                var actionExecuted = controllerType.GetMethod("OnActionExecuted", BindingFlags.NonPublic | BindingFlags.Instance);
                if (actionExecuted != null)
                {
                    var actionExecutedContext = MockActionExecutedContext.Create(controller, methodName);
                    actionExecuted.Invoke(controller, new object[] { actionExecutedContext });
                }

                if (returnObj?.Data == null)
                {
                    return default(TR);
                }

                Assert.IsAssignableFrom<TR>(returnObj.Data);

                return returnObj.Data as dynamic;
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException == null)
                {
                    return ex as dynamic;
                }

                if (ex.InnerException is FluentValidationException validationError)
                {
                    return validationError as dynamic;
                }

                throw ex.InnerException;
            }
        }
    }
}
