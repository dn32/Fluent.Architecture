#if NET461
using System;
using Fluent.Architecture.Exception;
using Fluent.Architecture.Exception.ValidationException;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Filters;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests;
using Fluent.Architecture.Test.Mock;
using Fluent.Architecture.Validation;
using Xunit;

#if NET461
using System.Web.Mvc;
#endif

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest
{
    public class ExceptionTest
    {
        [Fact]
        public void IncorrectDevelopmentExceptionTest()
        {
            var message = "test message";
            var incorrect = new IncorrectDevelopmentException(message);
            Assert.Equal(message, incorrect.Message);
        }

#if NET461
        [Theory]
        [InlineData(false, typeof(EntityExistsFluentValidationException))]
        [InlineData(false, typeof(PropertyNotNullFluentValidationException))]
        [InlineData(false, typeof(UniqueKeyFluentValidationException))]
        [InlineData(false, typeof(EntityNotFoundFluentValidationException))]
        [InlineData(false, typeof(FluentValidationException))]
        [InlineData(false, typeof(NullParameterFluentValidationException))]
        [InlineData(false, typeof(PropertyRequiredFluentValidationException))]

        [InlineData(true, typeof(EntityExistsFluentValidationException))]
        // For test custom errors
        public void ExceptionFilterTest(bool customErrorEnabled, Type exceptionType)
        {
            //var context = GetMockContext(customErrorEnabled, exceptionType);
            var controller = MockUtil.GetMockController(typeof(UserController));
            var filter = new ExceptionHandlerAttribute();
            var exception = new ContextFluentValidation();

            var parameters = exceptionType.GetConstructorParameters();
            parameters[0] = "Id";

            exception.AddInconsistency(Activator.CreateInstance(exceptionType, parameters) as FluentValidationException);

            var exceptionContext = MockUtil.GetMockExceptionContext(exception, controller, customErrorEnabled);
            
            filter.OnException(exceptionContext);

            if (customErrorEnabled)
            {
                Assert.IsAssignableFrom<EmptyResult>(exceptionContext.Result);
            }
            else
            {
                Assert.Equal(exception, ((JsonResult)exceptionContext.Result).Data);
            }
        }

#endif

    }
}
#endif
