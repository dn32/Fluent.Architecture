////-----------------------------------------------------------------------
////<copyright company = "Fluent System" >
////    Copyright © Fluent System.All rights reserved.
////    TODOS OS DIREITOS RESERVADOS.
////</copyright>
////-----------------------------------------------------------------------

//using System;
//using System.Runtime.InteropServices;
//using Fluent.Architecture.Entities;
//using Fluent.Architecture.Exceptions;
//using Fluent.Architecture.Exceptions.ValidationException;
//using Fluent.Architecture.Extensions;
//using Fluent.Architecture.Test;
//using Fluent.Architecture.Test.Mock;
//using Fluent.Architecture.Validation;
//using Microsoft.AspNetCore.Mvc;
//using NUnit.Framework;

//internal class ExceptionTest : InternalUserTestBase
//{
//    [Test]
//    public void IncorrectDevelopmentExceptionTest()
//    {
//        var message = "test message";
//        var incorrect = new IncorrectDevelopmentException(message);
//        Assert.AreEqual(message, incorrect.Message);
//    }

//    [Theory]
//    //[TestCase(typeof(PropertyRequiredFluentValidationException))]
//    //[TestCase(typeof(DbFieldRequiredFluentValidationException))]
//    //[TestCase(typeof(JsonFieldPropertyRequiredFluentValidationException))]
//    [TestCase(typeof(FluentUiFieldValidationException))]
//    [TestCase(typeof(UiFieldLenghtFluentValidationException))]
//    [TestCase(typeof(UiFieldRequiredFluentValidationException))]
//    public void ExceptionPropertyInfoFilterTest(Type exceptionType)
//    {
//        var filter = new FluentExceptionHandlerAttribute();
//        var exception = new ContextFluentValidationException();

//        var parameters = exceptionType.GetConstructorParameters();
//        parameters[0] = typeof(User).GetProperty(nameof(User.Name));

//        exception.AddInconsistency(Activator.CreateInstance(exceptionType, parameters) as FluentValidationException);
//        var controller = GetNewController();
//        var exceptionContext = MockUtil.GetMockExceptionContext<UserController>(
//            exception,
//            controller);

//        filter.OnException(exceptionContext);

//        var validationReturn = ((JsonResult)exceptionContext.Result).Value as ValidationReturn;
//        Assert.True(validationReturn?.ValidationError);
//        Assert.AreEqual(exception.Message, validationReturn?.Message);
//    }

//    [Theory]
//    //[TestCase(typeof(PropertyNotNullFluentValidationException))]
//    //[TestCase(typeof(UniqueKeyFluentValidationException))]
//    [TestCase(typeof(EntityExistsFluentValidationException))]
//    [TestCase(typeof(EntityNotFoundFluentValidationException))]
//    [TestCase(typeof(FluentValidationException))]
//    [TestCase(typeof(NullParameterFluentValidationException))]
//    [TestCase(typeof(MethodNotFoundException))]
//    public void ExceptionFilterTest(Type exceptionType)
//    {
//        var filter = new FluentExceptionHandlerAttribute();
//        var exception = new ContextFluentValidationException();

//        var parameters = exceptionType.GetConstructorParameters();
//        parameters[0] = "Id";

//        var inconsistency = Activator.CreateInstance(exceptionType, parameters) as FluentValidationException;
//        exception.AddInconsistency(inconsistency);

//        var controller = GetNewController();
//        var exceptionContext = MockUtil.GetMockExceptionContext<UserController>(
//            exception,
//            controller);

//        filter.OnException(exceptionContext);

//        var validationReturn = ((JsonResult)exceptionContext.Result).Value as ValidationReturn;
//        Assert.True(validationReturn?.ValidationError);
//        Assert.AreEqual(exception.Message, validationReturn?.Message);
//    }

//    [Test]
//    public void ExceptionFilter2Test()
//    {
//        var filter = new FluentExceptionHandlerAttribute();
//        var exception = new Exception("Test Exception");

//        var controller = GetNewController();
//        var exceptionContext = MockUtil.GetMockExceptionContext<UserController>(exception, controller);

//        filter.OnException(exceptionContext);
//        var data = ((JsonResult)exceptionContext.Result).Value;
//        FluentAssert.Equal("[{\"Name\":\"<Error>i__Field\",\"Value\":true},{\"Name\":\"<Message>i__Field\",\"Value\":\"Test Exception\"}]", data.GetAllDataOfObject());
//    }
//}

