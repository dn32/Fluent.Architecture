// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Sample.Test.SupportElements;
using Fluent.Architecture.Sample.Test.SupportElements.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Sample.Test.TestTools;
using Fluent.Architecture.Test;
using NUnit.Framework;

namespace Fluent.Architecture.Sample.Test.Test
{
    [TestFixture]
    [ComVisible(true)]
    internal class UserTest : FluentInternalTest
    {
        [Test]
        public void GetUserAndStudentByEmailTest()
        {
            var email = $"test{TestUtil.NextRandom()}@mail.com";
            var user = InternalTestUtil.GetNewUser();
            var student = InternalTestUtil.GetNewStudent();
            student.Email = email;
            user.Email = email;

            user = AddUser(user);
            student = AddStudent(student);

            var userStudent = TestUtil.ExecuteAndResult<UserStudent, UserController>(UserControllerInstance,(UserController controller) => controller.GetUserByEmail(email));

            Assert.NotNull(userStudent);
            FluentAssert.Equal(user, userStudent.User);
            FluentAssert.Equal(student, userStudent.Student);

            RemoveUser(user);
            RemoveStudent(student);
        }

        [Test]
        public void GetUserAndStudentByEmailJoinTest()
        {
            var email = $"test{TestUtil.NextRandom()}@mail.com";
            var user = InternalTestUtil.GetNewUser();
            var student = InternalTestUtil.GetNewStudent();
            student.Email = email;
            user.Email = email;

            user = AddUser(user);
            student = AddStudent(student);

            var userStudent = TestUtil.ExecuteAndResult<UserStudent, UserController>(UserControllerInstance,(UserController controller) => controller.SpecOneUserAndStudent(email));

            Assert.NotNull(userStudent);
            FluentAssert.Equal(user, userStudent.User);
            FluentAssert.Equal(student, userStudent.Student);

            RemoveUser(user);
            RemoveStudent(student);
        }

        [Theory]
        [TestCase(true)]
        [TestCase(false)]
        public void PropagationInCustomRepositorySuccessTest(bool found)
        {
            var user = InternalTestUtil.GetNewUser();
            user = AddUser(user);
            var userId = found ? user.Id : user.Id + TestUtil.NextRandom();
           
            var foundUser = TestUtil.ExecuteAndResult<User, UserController>(UserControllerInstance,(UserController controller) => controller.FindById(userId));

            if (found)
            {
                Assert.NotNull(foundUser);
                Assert.AreEqual(user.GetAllDataOfObject(), foundUser.GetAllDataOfObject());
            }
            else
            {
                Assert.Null(foundUser);
            }

            RemoveUser(user);
        }
    }
}
