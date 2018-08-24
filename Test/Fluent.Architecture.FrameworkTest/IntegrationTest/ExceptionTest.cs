//using System;
//using System.ComponentModel.DataAnnotations;
//using System.Web.Mvc;
//using System.Web.Routing;
//using Fluent.Architecture.Exception;
//using Fluent.Architecture.Exception.ValidationException;
//using Fluent.Architecture.Extensions;
//using Fluent.Architecture.Filters;
//using Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests.Controllers;
//using Fluent.Architecture.Test;
//using Fluent.Architecture.Test.Mock.ControllerMock;
//using Fluent.Architecture.Validation;
//using Xunit;

//namespace Fluent.Architecture.FrameworkTest.IntegrationTest
//{
//    public class ExceptionTest
//    {
//        [Fact]
//        public void IncorrectDevelopmentExceptionTest()
//        {
//            var message = "test message";
//            var incorrect = new IncorrectDevelopmentException(message);
//            Assert.Equal(message, incorrect.Message);
//        }

//        [Theory]
//        [InlineData(false, typeof(EntityExistsFluentValidationException))]
//        [InlineData(false, typeof(PropertyNotNullFluentValidationException))]
//        [InlineData(false, typeof(UniqueKeyFluentValidationException))]
//        [InlineData(false, typeof(EntityNotFoundFluentValidationException))]
//        [InlineData(false, typeof(FluentValidationException))]
//        [InlineData(false, typeof(NullParameterFluentValidationException))]
//        [InlineData(false, typeof(PropertyRequiredFluentValidationException))]

//        [InlineData(true, typeof(EntityExistsFluentValidationException))] // For test custom errors
//        public void ExceptionFilterTest(bool customErrorEnabled, Type exceptionType)
//        {
//            var controller = TestUtil.GetController(typeof(UserController));
//            controller.SetLocalHttpContext(new HttpContextBaseMock());

//            var parameters = exceptionType.GetConstructorParameters();
//            parameters[0] = "Id";

//            var exception = new ContextFluentValidation();
//            var exceptionInstance = Activator.CreateInstance(exceptionType, parameters);
//            exception.AddInconsistency(exceptionInstance as FluentValidationException);

//            var controllerContext = new ControllerContext(controller.HttpContext, new RouteData(), controller);
//            var context = new ExceptionContext(controllerContext, exception);
//            var filter = new ExceptionHandlerAttribute();
//            ((HttpContextBaseMock)controller.HttpContext).SetIsCustomErrorEnabled = customErrorEnabled;

//            filter.OnException(context);

//            if (customErrorEnabled)
//            {
//                Assert.IsAssignableFrom<EmptyResult>(context.Result);
//            }
//            else
//            {
//                Assert.Equal(exception, ((JsonResult)context.Result).Data);
//            }

//        }
//    }
//}
