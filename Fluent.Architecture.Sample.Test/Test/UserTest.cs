using System.Runtime.InteropServices;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Sample.Test.SupportElements;
using Fluent.Architecture.Sample.Test.SupportElements.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Sample.Test.SupportElements.Specifications;
using Fluent.Architecture.Sample.Test.TestTools;
using Fluent.Architecture.Test;
using NUnit.Framework;

namespace Fluent.Architecture.Sample.Test.Test
{
    [TestFixture]
    [ComVisible(true)]
    public class UserTest : FluentInternalTest
    {
        [Test]
        public void GetUserAndStudentByEmailTest()
        {
            var email = $"test{TestUtil.NextRandom()}@mail.com";
            var user = InternalTestUtil.GetNewUser();
            var student = InternalTestUtil.GetNewStudent();
            student.Email = email;
            user.Email = email;

            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Add), user);
            student = TestUtil.Execute<Student>(this.StudentControllerInstance, nameof(UserController.Add), student);

            var userStudent = TestUtil.Execute<UserStudent>(this.UserControllerInstance, nameof(UserController.GetUserByEmail), email);

            Assert.NotNull(userStudent);
            FluentAssert.Equal(user, userStudent.User);
            FluentAssert.Equal(student, userStudent.Student);

            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Remove), user);
            TestUtil.Execute<Student>(this.StudentControllerInstance, nameof(StudentController.Remove), student);
        }

        [Test]
        public void GetUserAndStudentByEmailJoinTest()
        {
            var email = $"test{TestUtil.NextRandom()}@mail.com";
            var user = InternalTestUtil.GetNewUser();
            var student = InternalTestUtil.GetNewStudent();
            student.Email = email;
            user.Email = email;

            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Add), user);
            student = TestUtil.Execute<Student>(this.StudentControllerInstance, nameof(UserController.Add), student);

            var spec = new UserAndStudentByEmail(this.UserControllerInstance, email);
            var userStudent = TestUtil.Execute<UserStudent>(this.UserControllerInstance, nameof(UserController.SpecOne), spec);

            Assert.NotNull(userStudent);
            FluentAssert.Equal(user, userStudent.User);
            FluentAssert.Equal(student, userStudent.Student);

            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Remove), user);
            TestUtil.Execute<Student>(this.StudentControllerInstance, nameof(StudentController.Remove), student);
        }

        [Theory]
        [TestCase(true)]
        [TestCase(false)]
        public void PropagationInCustomRepositorySuccessTest(bool found)
        {
            var user = InternalTestUtil.GetNewUser();
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Add), user);
            var userId = found ? user.Id : user.Id + TestUtil.NextRandom();
            var foundUser = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.FindById), userId);

            if (found)
            {
                Assert.NotNull(foundUser);
                Assert.AreEqual(user.GetAllDataOfObject(), foundUser.GetAllDataOfObject());
            }
            else
            {
                Assert.Null(foundUser);
            }

            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Remove), user);
        }
    }
}
