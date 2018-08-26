using Fluent.Architecture.Attributes;
using Fluent.Architecture.Exception;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.StudentTests;
using Xunit;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests
{
    public class UserTest
    {
        #region SETUP

        public UserTest()
        {
            Setup.Initialize();
        }

        #endregion

        [Fact]
        public void GetUserByEmailTest()
        {
            var email = $"test{TestUtil.NextRandom()}@mail.com";
            var user = UserTestUtil.GetNew();
            var student = StudentTestUtil.GetNew();
            student.Email = email;
            user.Email = email;

            user = TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Add), user);
            student = TestUtil.Execute<Student>(typeof(StudentController), nameof(UserController.Add), student);

            var userStudent = TestUtil.Execute<UserStudent>(typeof(UserController), nameof(UserController.GetUserByEmail), email);

            Assert.NotNull(userStudent);
            Assert.Equal(user.GetAllDataOfObject(), userStudent.User.GetAllDataOfObject());
            Assert.Equal(student.GetAllDataOfObject(), userStudent.Student.GetAllDataOfObject());

            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Remove), user);
            TestUtil.Execute<Student>(typeof(StudentController), nameof(StudentController.Remove), student);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PropagationInCustomRepositorySucessTest(bool found)
        {
            var user = UserTestUtil.GetNew();
            user = TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Add), user);
            var userId = found ? user.Id : user.Id + TestUtil.NextRandom();
            var foundUser = TestUtil.Execute<User>(typeof(UserController), nameof(UserController.FindById), userId);

            if (found)
            {
                Assert.NotNull(foundUser);
                Assert.Equal(user.GetAllDataOfObject(), foundUser.GetAllDataOfObject());
            }
            else
            {
                Assert.Null(foundUser);
            }

            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Remove), user);
        }

        [Fact]
        public void PropagationMethodNotFoundFailTest()
        {
            var ex = Assert.Throws<IncorrectDevelopmentException>(() => TestUtil.Execute<User>(typeof(UserController), nameof(UserController.NotFound)));
            Assert.NotNull(ex);
            Assert.Equal("The NotFound method was not found in the service Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.User and repository Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.User", ex.Message);
        }


        [Fact]
        public void PropagationMethodNotFound2FailTest()
        {
            var ex = Assert.Throws<IncorrectDevelopmentException>(() => TestUtil.Execute<User>(typeof(UserController), nameof(UserController.NotFound2)));
            Assert.NotNull(ex);
            Assert.Equal($"A propagation request was unsuccessful.\nThe request does not indicate the method name and could not be obtained by reflection.\nMake sure that the method that calls the propagation is not decorated with { nameof(NotPropagateAttribute)}, as it should not be.", ex.Message);
        }
    }
}
