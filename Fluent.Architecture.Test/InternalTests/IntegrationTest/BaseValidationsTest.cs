#if NET461

using System;
using System.Linq;
using Fluent.Architecture.Exception.ValidationException;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests;
using Fluent.Architecture.Validation;
using Xunit;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest
{
    public class BaseValidationsTest
    {
#region SETUP

        public BaseValidationsTest()
        {
            Setup.Initialize();
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
            var user = UserTestUtil.GetNew();

            if (method == nameof(UserController.Update))
            {
                user = TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Add), user);
            }

            user.Name = name;

            var error = TestUtil.Execute<ContextFluentValidation>(typeof(UserController), method, user);

            Assert.NotNull(error);
            Assert.Single(error.Inconsistencies);
            Assert.IsAssignableFrom<PropertyRequiredFluentValidationException>(error.Inconsistencies.First());

            if (method == nameof(UserController.Update))
            {
                TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Remove), user);
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
            var user = UserTestUtil.GetNew();

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
    }
}
#endif
