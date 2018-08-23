using System;
using System.Configuration;
using System.Linq;
using Fluent.Architecture.Exception.ValidationException;
using Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests.Controllers;
using Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests.Enum;
using Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests.Models;
using Fluent.Architecture.Test;
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

        // Null parameter
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

        // Required required null  error
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void RequiredAddTestFail(string name)
        {
            var user = new User()
            {
                Name = name
            };

            var error = TestUtil.Execute<ContextFluentValidation>(typeof(UserController), nameof(UserController.Add), user);

            Assert.NotNull(error);
            Assert.Equal(2, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<PropertyRequiredFluentValidationException>(error.Inconsistencies.First());
            Assert.IsAssignableFrom<PropertyRequiredFluentValidationException>(error.Inconsistencies.Last());
        }

        // Required required null error
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void RequiredUpdateTestFail(string name)
        {
            var user = new User
            {
                Name = "Maria",
                Email = $"maria{Guid.NewGuid()}@mail.com",
                PersonType = ePersonType.User,
                Id = new Random().Next(1, int.MaxValue)
            };

            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Add), user);
            user.Name = name;

            var error = TestUtil.Execute<ContextFluentValidation>(typeof(UserController), nameof(UserController.Update), user);

            Assert.NotNull(error);
            Assert.Single(error.Inconsistencies);
            Assert.IsAssignableFrom<PropertyRequiredFluentValidationException>(error.Inconsistencies.First());

            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Remove), user);
        }

        [Fact]
        public void AddErrorFluentUniqueKey()
        {
            var user = new User
            {
                Name = "Maria",
                Email = $"maria{Guid.NewGuid()}@mail.com",
                PersonType = ePersonType.User,
                Id = new Random().Next(1, int.MaxValue)
            };

            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Add), user);

            user.Id = new Random().Next(1, int.MaxValue);

            var objectReturn = TestUtil.Execute<ContextFluentValidation>(typeof(UserController), nameof(UserController.Add), user);
            Assert.NotNull(objectReturn);
            Assert.Single(objectReturn.Inconsistencies);
            Assert.IsAssignableFrom<UniqueKeyFluentValidationException>(objectReturn.Inconsistencies.First());
        }

        // Update and remove not found fail
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

        // Update and remove not key
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


        // Add and remove success
        [Fact]
        public void AddAndRemoveSuccess()
        {
            var user = new User
            {
                Name = $"test{Guid.NewGuid()}@mail.com",
                PersonType = ePersonType.User,
                Id = new Random().Next(1, int.MaxValue)
            };

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
