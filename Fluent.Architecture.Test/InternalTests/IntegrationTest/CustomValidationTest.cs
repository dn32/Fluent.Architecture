#if NET461
using System.Linq;
using Fluent.Architecture.Exception.ValidationException;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests;
using Fluent.Architecture.Validation;
using Xunit;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest
{
    public class CustomValidationTest
    {
        #region SETUP

        public CustomValidationTest()
        {
            Setup.Initialize();
        }

        #endregion

        #region FAIL

        [Fact]
        public void FullNameAddValidationFail()
        {
            var user = UserTestUtil.GetNew();
            user.Name = "Maria";

            var error = TestUtil.Execute<ContextFluentValidation>(typeof(UserController), nameof(UserController.Add), user);
            Assert.NotNull(error);
            Assert.Single(error.Inconsistencies);
            Assert.IsAssignableFrom<FluentPropertyValidationException>(error.Inconsistencies.First());
            Assert.Equal(nameof(User.Name), ((FluentPropertyValidationException)error.Inconsistencies.First()).Property);
        }

        [Fact]
        public void FullNameUpdateValidationFail()
        {
            var user = UserTestUtil.GetNew();
            user.Name = "Maria Santos";

            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Add), user);
            user = TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Find), user);
            Assert.NotNull(user);

            //Update
            user.Name = "Maria";
            var error = TestUtil.Execute<ContextFluentValidation>(typeof(UserController), nameof(UserController.Update), user);
            Assert.NotNull(error);
            Assert.Single(error.Inconsistencies);
            Assert.IsAssignableFrom<FluentPropertyValidationException>(error.Inconsistencies.First());
            Assert.Equal(nameof(User.Name), ((FluentPropertyValidationException)error.Inconsistencies.First()).Property);

            //Remove
            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Remove), user);
            user = TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Find), user);
            Assert.Null(user);
        }

#endregion

#region SUCCESS

        [Fact]
        public void FullNameAddAndUpdateValidationSuccess()
        {
            var user = UserTestUtil.GetNew();
            user.Name = "Maria Santos";

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
