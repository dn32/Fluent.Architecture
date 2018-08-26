using Fluent.Architecture.Attributes;
using Fluent.Architecture.Exception;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Test.SupportElements.Controllers;
using Fluent.Architecture.Test.SupportElements.Model;
using Fluent.Architecture.Test.SupportElements.Services;
using Fluent.Architecture.Test.SupportElements.Specifications;
using Fluent.Architecture.Test.TestTools;
using Xunit;

namespace Fluent.Architecture.Test.Test
{
    public class UserTest : FluentInternalTest
    {
        [Fact]
        public void GetUserAndStudentByEmailTest()
        {
            var email = $"test{TestUtil.NextRandom()}@mail.com";
            var user = InternalTestUtil.GetNewUser();
            var student = InternalTestUtil.GetNewStudent();
            student.Email = email;
            user.Email = email;

            user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Add), user);
            student = TestUtil.Execute<Student>(StudentControllerInstance, nameof(UserController.Add), student);

            var userStudent = TestUtil.Execute<UserStudent>(UserControllerInstance, nameof(UserController.GetUserByEmail), email);

            Assert.NotNull(userStudent);
            FluentAssert.Equal(user, userStudent.User);
            FluentAssert.Equal(student, userStudent.Student);

            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Remove), user);
            TestUtil.Execute<Student>(StudentControllerInstance, nameof(StudentController.Remove), student);
        }

        [Fact]
        public void GetUserAndStudentByEmailJoinTest()
        {
            var email = $"test{TestUtil.NextRandom()}@mail.com";
            var user = InternalTestUtil.GetNewUser();
            var student = InternalTestUtil.GetNewStudent();
            student.Email = email;
            user.Email = email;

            user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Add), user);
            student = TestUtil.Execute<Student>(StudentControllerInstance, nameof(UserController.Add), student);

            var spec = new UserAndStudentByEmail(UserControllerInstance, email);
            var userStudent = TestUtil.Execute<UserStudent>(UserControllerInstance, nameof(UserController.SpecOne), spec);

            Assert.NotNull(userStudent);
            FluentAssert.Equal(user, userStudent.User);
            FluentAssert.Equal(student, userStudent.Student);

            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Remove), user);
            TestUtil.Execute<Student>(StudentControllerInstance, nameof(StudentController.Remove), student);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PropagationInCustomRepositorySuccessTest(bool found)
        {
            var user = InternalTestUtil.GetNewUser();
            user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Add), user);
            var userId = found ? user.Id : user.Id + TestUtil.NextRandom();
            var foundUser = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.FindById), userId);

            if (found)
            {
                Assert.NotNull(foundUser);
                Assert.Equal(user.GetAllDataOfObject(), foundUser.GetAllDataOfObject());
            }
            else
            {
                Assert.Null(foundUser);
            }

            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Remove), user);
        }

        [Fact]
        public void PropagationMethodNotFoundFailTest()
        {
            var ex = Assert.Throws<IncorrectDevelopmentException>(() => TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.NotFound)));
            Assert.NotNull(ex);
            Assert.Equal("The NotFound method was not found in the service Fluent.Architecture.Test.SupportElements.User and repository Fluent.Architecture.Test.SupportElements.User", ex.Message);
        }

        [Fact]
        public void PropagationMethodNotFound2FailTest()
        {
            var ex = Assert.Throws<IncorrectDevelopmentException>(() => TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.NotFound2)));
            Assert.NotNull(ex);
            Assert.Equal($"A propagation request was unsuccessful.\nThe request does not indicate the method name and could not be obtained by reflection.\nMake sure that the method that calls the propagation is not decorated with { nameof(NotPropagateAttribute)}, as it should not be.", ex.Message);
        }
    }
}
