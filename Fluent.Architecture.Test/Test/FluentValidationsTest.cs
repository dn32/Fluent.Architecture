#if NET461

using System;
using System.Linq;
using Fluent.Architecture.Exception.ValidationException;
using Fluent.Architecture.Test.SupportElements.Controllers;
using Fluent.Architecture.Test.SupportElements.Mock;
using Fluent.Architecture.Test.SupportElements.Model;
using Fluent.Architecture.Test.TestTools;
using Fluent.Architecture.Validation;
using Xunit;

namespace Fluent.Architecture.Test.Test
{
    public class FluentValidationsTest : FluentInternalTest
    {
        [Theory]
        [InlineData(nameof(UserController.Add))]
        [InlineData(nameof(UserController.Update))]
        public void NullParameterTestFail(string method)
        {
            var error = TestUtil.Execute<ContextFluentValidation>(UserControllerInstance, method, new object[] { null });

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
            var user = InternalTestUtil.GetNewUser();

            if (method == nameof(UserController.Update))
            {
                user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Add), user);
            }

            user.Name = name;

            var error = TestUtil.Execute<ContextFluentValidation>(UserControllerInstance, method, user);

            Assert.NotNull(error);
            Assert.Single(error.Inconsistencies);
            Assert.IsAssignableFrom<PropertyRequiredFluentValidationException>(error.Inconsistencies.First());

            if (method == nameof(UserController.Update))
            {
                TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Remove), user);
            }
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

            var err = TestUtil.Execute<ContextFluentValidation>(UserControllerInstance, method, user);
            Assert.NotNull(err);
            Assert.Single(err.Inconsistencies);
            Assert.IsAssignableFrom<EntityNotFoundFluentValidationException>(err.Inconsistencies.First());
            Assert.True(err.ValidationError);
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

            var objectReturn = TestUtil.Execute<ContextFluentValidation>(UserControllerInstance, method, user);
            Assert.NotNull(objectReturn);
            Assert.Single(objectReturn.Inconsistencies);
            Assert.IsAssignableFrom<PropertyRequiredFluentValidationException>(objectReturn.Inconsistencies.First());

            objectReturn.Inconsistencies.Clear();
        }

        [Fact]
        public void AddUpdateAndRemoveSuccess()
        {
            var user = InternalTestUtil.GetNewUser();

            //Add
            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Add), user);
            user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Find), user);
            Assert.NotNull(user);

            //Update
            user.Name = "New name";
            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Update), user);
            user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Find), user);
            Assert.NotNull(user);
            Assert.Equal("New name", user.Name);

            //Remove
            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Remove), user);
            user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Find), user);
            Assert.Null(user);
        }

        [Fact]
        public void FullNameAddValidationFail()
        {
            var user = InternalTestUtil.GetNewUser();
            user.Name = "Maria";

            var error = TestUtil.Execute<ContextFluentValidation>(UserControllerInstance, nameof(UserController.Add), user);
            Assert.NotNull(error);
            Assert.Single(error.Inconsistencies);
            Assert.IsAssignableFrom<FluentPropertyValidationException>(error.Inconsistencies.First());
            Assert.Equal(nameof(User.Name), ((FluentPropertyValidationException)error.Inconsistencies.First()).Property);
        }

        [Fact]
        public void FullNameUpdateValidationFail()
        {
            var user = InternalTestUtil.GetNewUser();
            user.Name = "Maria Santos";

            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Add), user);
            user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Find), user);
            Assert.NotNull(user);

            //Update
            user.Name = "Maria";
            var error = TestUtil.Execute<ContextFluentValidation>(UserControllerInstance, nameof(UserController.Update), user);
            Assert.NotNull(error);
            Assert.Single(error.Inconsistencies);
            Assert.IsAssignableFrom<FluentPropertyValidationException>(error.Inconsistencies.First());
            Assert.Equal(nameof(User.Name), ((FluentPropertyValidationException)error.Inconsistencies.First()).Property);

            //Remove
            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Remove), user);
            user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Find), user);
            Assert.Null(user);
        }

        [Fact]
        public void FullNameAddAndUpdateValidationSuccess()
        {
            var user = InternalTestUtil.GetNewUser();
            user.Name = "Maria Santos";

            //Add
            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Add), user);
            user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Find), user);
            Assert.NotNull(user);

            //Update
            user.Name = "New name";
            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Update), user);
            user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Find), user);
            Assert.NotNull(user);
            Assert.Equal("New name", user.Name);

            //Remove
            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Remove), user);
            user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Find), user);
            Assert.Null(user);
        }

    }
}
#endif
