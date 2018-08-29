#if NET461

using System;
using System.Web.Mvc;

using Fluent.Architecture.Extensions;
using Fluent.Architecture.Filters;
using Fluent.Architecture.Test.SupportElements;
using Fluent.Architecture.Test.SupportElements.Controllers;
using Fluent.Architecture.Test.SupportElements.Mock;
using Fluent.Architecture.Validation;
using NUnit.Framework;
using System.Runtime.InteropServices;
using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Exceptions.ValidationException;

namespace Fluent.Architecture.Test.Test
{
    [TestFixture]
    [ComVisible(true)]
    public class ExceptionTest : FluentInternalTest
    {
        [Test]
        public void IncorrectDevelopmentExceptionTest()
        {
            var message = "test message";
            var incorrect = new IncorrectDevelopmentException(message);
            Assert.AreEqual(message, incorrect.Message);
        }

        [Theory]
        [TestCase(false, typeof(EntityExistsFluentValidationException))]
        [TestCase(false, typeof(PropertyNotNullFluentValidationException))]
        [TestCase(false, typeof(UniqueKeyFluentValidationException))]
        [TestCase(false, typeof(EntityNotFoundFluentValidationException))]
        [TestCase(false, typeof(FluentValidationException))]
        [TestCase(false, typeof(NullParameterFluentValidationException))]
        [TestCase(false, typeof(PropertyRequiredFluentValidationException))]
        [TestCase(true, typeof(EntityExistsFluentValidationException))]

        // For test custom errors
        public void ExceptionFilterTest(bool customErrorEnabled, Type exceptionType)
        {
            var filter = new ExceptionHandlerAttribute();
            var exception = new ContextFluentValidationException();

            var parameters = exceptionType.GetConstructorParameters();
            parameters[0] = "Id";

            exception.AddInconsistency(
                Activator.CreateInstance(exceptionType, parameters) as FluentValidationException);

            var exceptionContext = MockUtil.GetMockExceptionContext<UserController>(
                exception,
                this.UserControllerInstance,
                customErrorEnabled);

            filter.OnException(exceptionContext);

            if (customErrorEnabled)
            {
                Assert.IsAssignableFrom<EmptyResult>(exceptionContext.Result);
            }
            else
            {
                Assert.AreEqual(exception, ((JsonResult)exceptionContext.Result).Data);
            }
        }

        [Test]
        public void ExceptionFilter2Test()
        {
            var filter = new ExceptionHandlerAttribute();
            var exception = new Exception("Test Exception");

            var exceptionContext = MockUtil.GetMockExceptionContext<UserController>(exception, this.UserControllerInstance, false);

            filter.OnException(exceptionContext);
            var data = ((JsonResult)exceptionContext.Result).Data;
            FluentAssert.Equal("[{\"Name\":\"<Error>i__Field\",\"Value\":true},{\"Name\":\"<Message>i__Field\",\"Value\":\"Test Exception\"}]", data.GetAllDataOfObject());
        }
    }
}
#endif
