// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Exceptions.ValidationException;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Filters;
using Fluent.Architecture.Sample.Test.SupportElements;
using Fluent.Architecture.Sample.Test.SupportElements.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Test;
using Fluent.Architecture.Test.Mock;
using Fluent.Architecture.Validation;
using NUnit.Framework;

namespace Fluent.Architecture.Sample.Test.Test
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
        [TestCase(false, typeof(PropertyRequiredFluentValidationException))]
        [TestCase(false, typeof(DbFieldRequiredFluentValidationException))]
        [TestCase(false, typeof(JsonFieldPropertyRequiredFluentValidationException))]
        [TestCase(false, typeof(UiFieldRequiredFluentValidationException))]
        [TestCase(false, typeof(UiFieldRequiredFluentValidationException))]
        [TestCase(true, typeof(UiFieldMaxLenghtFluentValidationException))]

        // For test custom errors
        public void ExceptionPropertyInfoFilterTest(bool customErrorEnabled, Type exceptionType)
        {
            var filter = new FluentExceptionHandlerAttribute();
            var exception = new ContextFluentValidationException();

            var parameters = exceptionType.GetConstructorParameters();
            parameters[0] = typeof(Course).GetProperty(nameof(Course.Description));

            exception.AddInconsistency(Activator.CreateInstance(exceptionType, parameters) as FluentValidationException);

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
                var validationReturn = ((JsonResult)exceptionContext.Result).Data as ValidationReturn;
                Assert.True(validationReturn?.ValidationError);
                Assert.AreEqual(exception.Message, validationReturn?.Message);
            }
        }

        [Theory]
        [TestCase(false, typeof(EntityExistsFluentValidationException))]
        [TestCase(false, typeof(PropertyNotNullFluentValidationException))]
        [TestCase(false, typeof(UniqueKeyFluentValidationException))]
        [TestCase(false, typeof(EntityNotFoundFluentValidationException))]
        [TestCase(false, typeof(FluentValidationException))]
        [TestCase(false, typeof(NullParameterFluentValidationException))]
        [TestCase(true, typeof(MethodNotFoundException))]

        // For test custom errors
        public void ExceptionFilterTest(bool customErrorEnabled, Type exceptionType)
        {
            var filter = new FluentExceptionHandlerAttribute();
            var exception = new ContextFluentValidationException();

            var parameters = exceptionType.GetConstructorParameters();
            parameters[0] = "Id";

            exception.AddInconsistency(Activator.CreateInstance(exceptionType, parameters) as FluentValidationException);

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
                var validationReturn = ((JsonResult)exceptionContext.Result).Data as ValidationReturn;
                Assert.True(validationReturn?.ValidationError);
                Assert.AreEqual(exception.Message, validationReturn?.Message);
            }
        }
        
        [Test]
        public void ExceptionFilter2Test()
        {
            var filter = new FluentExceptionHandlerAttribute();
            var exception = new Exception("Test Exception");

            var exceptionContext = MockUtil.GetMockExceptionContext<UserController>(exception, this.UserControllerInstance, false);

            filter.OnException(exceptionContext);
            var data = ((JsonResult)exceptionContext.Result).Data;
            FluentAssert.Equal("[{\"Name\":\"<Error>i__Field\",\"Value\":true},{\"Name\":\"<Message>i__Field\",\"Value\":\"Test Exception\"}]", data.GetAllDataOfObject());
        }
    }
}

