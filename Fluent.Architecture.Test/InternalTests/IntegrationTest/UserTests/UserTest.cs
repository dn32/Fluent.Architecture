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
    }
}
