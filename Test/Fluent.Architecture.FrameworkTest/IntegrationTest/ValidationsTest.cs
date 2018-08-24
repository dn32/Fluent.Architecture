using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Fluent.Architecture.Exception;
using Fluent.Architecture.Exception.ValidationException;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Filters;
using Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests.Controllers;
using Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests.Enum;
using Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests.Models;
using Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests.Services;
using Fluent.Architecture.Test;
using Fluent.Architecture.Test.Mock;
using Fluent.Architecture.Test.Mock.ControllerMock;
using Fluent.Architecture.Validation;
using Xunit;

namespace Fluent.Architecture.FrameworkTest.IntegrationTest
{
    public class ValidationsTest
    {
        #region SETUP

        public ValidationsTest()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Setup.Initialize(connectionString);
        }

        #endregion

        #region FAIL

        [Theory]
        [InlineData(nameof(UserController.Add))]
        [InlineData(nameof(UserController.Update))]
        public void NullParameterTestFail(string method)
        {
            var error = TestUtil.Execute<ContextFluentValidation>(typeof(UserController), method, null);

            Assert.NotNull(error);
            Assert.Single(error.Inconsistencies);
            Assert.IsAssignableFrom<NullParameterFluentValidationException>(error.Inconsistencies.First());
        }

        [Theory]
        [InlineData(nameof(UserController.Update), "")]
        [InlineData(nameof(UserController.Add), "")]
        [InlineData(nameof(UserController.Update), " ")]
        [InlineData(nameof(UserController.Add), " ")]
        [InlineData(nameof(UserController.Update), null)]
        [InlineData(nameof(UserController.Add), null)]
        public void RequiredAddAndUpdateTestFail(string method, string name)
        {
            var user = GetNewUser();

            if (method == nameof(UserController.Update))
            {
                user = TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Add), user);
            }

            user.Name = name;

            var error = TestUtil.Execute<ContextFluentValidation>(typeof(UserController), method, user);

            Assert.NotNull(error);
            Assert.Single(error.Inconsistencies);
            Assert.IsAssignableFrom<PropertyRequiredFluentValidationException>(error.Inconsistencies.First());

            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Remove), user);
        }

        [Theory]
        [InlineData(nameof(UserController.Update))]
        [InlineData(nameof(UserController.Remove))]
        public void UpdateAndRemoveNotFoundFail(string method)
        {
            var user = new User
            {
                Name = $"test{Guid.NewGuid()}@mail.com",
                PersonType = ePersonType.User,
                Id = new Random().Next(1, int.MaxValue)
            };

            var objectReturn = TestUtil.Execute<ContextFluentValidation>(typeof(UserController), method, user);
            Assert.NotNull(objectReturn);
            Assert.Single(objectReturn.Inconsistencies);
            Assert.IsAssignableFrom<EntityNotFoundFluentValidationException>(objectReturn.Inconsistencies.First());
        }

        [Theory]
        [InlineData(nameof(UserController.Update))]
        [InlineData(nameof(UserController.Remove))]
        public void UpdateAndRemoveNotKeyValueFail(string method)
        {
            var user = new User
            {
                Name = $"test{Guid.NewGuid()}@mail.com",
                PersonType = ePersonType.User,
            };

            var objectReturn = TestUtil.Execute<ContextFluentValidation>(typeof(UserController), method, user);
            Assert.NotNull(objectReturn);
            Assert.Single(objectReturn.Inconsistencies);
            Assert.IsAssignableFrom<PropertyRequiredFluentValidationException>(objectReturn.Inconsistencies.First());

            objectReturn.Inconsistencies.Clear();
        }

        #endregion

        #region SUCCESS

        [Fact]
        public void AddUpdateAndRemoveSuccess()
        {
            var user = GetNewUser();

            //Add
            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Add), user);
            user = TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Find), user);
            Assert.NotNull(user);

            //Update
            user.Name = "New name";
            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Update), user);
            user = TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Find), user);
            Assert.NotNull(user);
            Assert.Equal("New name", user.Name);

            //Remove
            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Remove), user);
            user = TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Find), user);
            Assert.Null(user);
        }

        #endregion

        #region INTERNAL UTILS

        private User GetNewUser()
        {
            var rand = new Random().Next(1, int.MaxValue);
            return new User
            {
                PersonType = ePersonType.User,
                Id = rand,
                UserName = $"maria {rand}",
                Name = $"maria {rand}",
                Email = $"test{rand}@mail.com",
                Password = $"test{rand}@mail.com",
                Tel = $"test{rand}@mail.com",
            };
        }

        #endregion
    }

    public class ExceptionTest
    {
        [Fact]
        public void IncorrectDevelopmentExceptionTest()
        {
            var message = "test message";
            var incorrect = new IncorrectDevelopmentException(message);
            Assert.Equal(message, incorrect.Message);
        }

        [Fact]
        public void ExceptionFilterTest()
        {
            var controller = TestUtil.GetController(typeof(UserController));
            controller.SetLocalHttpContext(new HttpContextBaseMock());

            var exception = new ContextFluentValidation();
            exception.AddInconsistency(new EntityExistsFluentValidationException("Id"));

            var controllerContext = new ControllerContext(controller.HttpContext,new RouteData(), controller);
            var context = new ExceptionContext(controllerContext, exception);
            var filter = new ExceptionHandlerAttribute();
            ((HttpContextBaseMock)controller.HttpContext).SetIsCustomErrorEnabled = true;

            filter.OnException(context);
            Assert.Equal(exception, ((JsonResult)context.Result).Data);
        }
    }
}
