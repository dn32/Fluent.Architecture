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
        public ValidationsTest()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Setup.Initialize(connectionString);
        }

        // Objeto nulo
        [Theory]
        [InlineData(nameof(UserController.Add))]
        [InlineData(nameof(UserController.Update))]
        public void NullParameterTestFail(string method)
        {
            var error = TestUtil.Execute<ContextValidation>(typeof(UserController), method, null);

            Assert.NotNull(error);
            Assert.Single(error.Inconsistencies);
            Assert.IsAssignableFrom<NullParameterFluentValidationtException>(error.Inconsistencies.First());
        }

        // Required erro
        [Fact]
        public void RequiredAddTestFail()
        {
            var user = new User();
            var error = TestUtil.Execute<ContextValidation>(typeof(UserController), nameof(UserController.Add), user);

            Assert.NotNull(error);
            Assert.Equal(2, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<PropertyRequiredFluentValidationtException>(error.Inconsistencies.First());
            Assert.IsAssignableFrom<PropertyRequiredFluentValidationtException>(error.Inconsistencies.Last());
        }      
        
        // Required erro
        [Fact]
        public void RequiredUpdateTestFail()
        {
            var user = new User
            {
                Name = "Maria",
                Email = $"maria{Guid.NewGuid()}@mail.com",
                PersonType = ePersonType.User,
                Id = new Random().Next(1, int.MaxValue)
            };

            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Add), user);
            user.Name = null;

            var error = TestUtil.Execute<ContextValidation>(typeof(UserController), nameof(UserController.Update), user);

            Assert.NotNull(error);
            Assert.Single(error.Inconsistencies);
            Assert.IsAssignableFrom<PropertyRequiredFluentValidationtException>(error.Inconsistencies.First());

            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Remove), user);
        }

        [Fact]
        public void AddErrorFluentUnicKey()
        {
            var user = new User
            {
                Name = "Maria",
                Email = $"maria{Guid.NewGuid()}@mail.com",
                PersonType = ePersonType.User,
                Id = new Random().Next(1, int.MaxValue)
            };

            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Add), user);

            user.Id = 0;

            var objectReturn = TestUtil.Execute<ContextValidation>(typeof(UserController), nameof(UserController.Add), user);
            Assert.NotNull(objectReturn);
            Assert.Single(objectReturn.Inconsistencies);
            Assert.IsAssignableFrom<UnicKeyFluentValidationtException>(objectReturn.Inconsistencies.First());
        }

        // Add ok
        [Fact]
        public void AddTestOk()
        {
            var user = new User
            {
                Name = $"test{Guid.NewGuid()}@mail.com",
                PersonType = ePersonType.User,
                Id = new Random().Next(1, int.MaxValue)
            };

            var userAdded = TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Add), user);

            Assert.NotNull(userAdded);

            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Remove), user);
        }
    }
}
