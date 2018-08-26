// ReSharper disable CommentTypo

using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Exception.ValidationException;
using Fluent.Architecture.Service;
using Fluent.Architecture.Test.SupportElements.Mock;
using Fluent.Architecture.Test.SupportElements.Mock.ControllerMock;
using Xunit;

namespace Fluent.Architecture.Test.TestTools
{
    internal static class TestUtil
    {
        public static BaseController GetController(Type controllerType)
        {
            return ControllerMockFactory.Create(controllerType);
        }

        private static readonly Random Random = new Random();

        private static readonly object SyncLock = new object();

        public static int NextRandom()
        {
            lock (SyncLock)
            {
                return Random.Next(1, int.MaxValue);
            }
        }

#if NET461
        public static TR Execute<TR>(Type controllerType, string methodName, params object[] parameters)
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
                var parameterTypes = (from parameter in parameters select parameter == null ? typeof(object) : parameter.GetType()).ToList();
                method = controllerType.GetMethod(methodName, parameterTypes.ToArray());
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

                if (returnObj?.Data == null)
                {
                    return default(TR);
                }

                Assert.IsAssignableFrom<TR>(returnObj.Data);

                return returnObj?.Data as dynamic;
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
#else
        public static TR Execute<TR>(Type controllerType, string methodName, params object[] parameters) where TR : class
        {
            throw new NotImplementedException();
        }
#endif
    }
}
