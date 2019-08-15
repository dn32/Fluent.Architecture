//// -----------------------------------------------------------------------
//// <copyright company="Fluent System">
////     Copyright © Fluent System. All rights reserved.
////     TODOS OS DIREITOS RESERVADOS.
//// </copyright>
//// -----------------------------------------------------------------------

//using System.Runtime.InteropServices;
//using Fluent.Architecture.Extensions;
//using Fluent.Architecture.Sample.Test.SupportElements;
//using Fluent.Architecture.Sample.Test.SupportElements.Controllers;
//using Fluent.Architecture.Sample.Test.SupportElements.Model;
//using Fluent.Architecture.Sample.Test.TestTools;
//using Fluent.Architecture.Test;
//using NUnit.Framework;

//namespace Fluent.Architecture.Sample.Test.Test
//{
//    [TestFixture]
//    [ComVisible(true)]
//    internal class UserTest : FluentInternalTest
//    {
//        //[Test]
//        //public void GetUserAndStudentByEmailTest()
//        //{
//        //    var email = $"test{TestUtil.NextRandom()}@mail.com";
//        //    var user = InternalTestUtil.GetNewUser();
//        //    var student = InternalTestUtil.GetNewStudent();
//        //    student.Email = email;
//        //    user.Email = email;

//        //    AddUser(user);
//        //    AddStudent(student);

//        //    user = FindUser(user);
//        //    student = FindStudent(student);

//        //    var userStudent = TestUtil.Execute<UserController, UserStudent>(UserControllerInstance, (UserController controller) => controller.GetUserByEmail(email));

//        //    Assert.NotNull(userStudent);
//        //    FluentAssert.Equal(user, userStudent.User);
//        //    FluentAssert.Equal(student, userStudent.Student);

//        //    RemoveUser(user);
//        //    RemoveStudent(student);
//        //}

//        [Test]
//        public void GetUserAndStudentByEmailJoinTest()
//        {
//            var email = $"test{TestUtil.NextRandom()}@mail.com";
//            var user = InternalTestUtil.GetNewUser();
//            var student = InternalTestUtil.GetNewStudent();
//            student.Email = email;
//            user.Email = email;

//            AddUser(user);
//            AddStudent(student);

//            user = FindUser(user);
//            student = FindStudent(student);

//            var userStudent = TestUtil.Execute<UserController, UserStudent>(UserControllerInstance,(UserController controller) => controller.SpecOneUserAndStudent(email));

//            Assert.NotNull(userStudent);
//            FluentAssert.Equal(user, userStudent.User);
//            FluentAssert.Equal(student, userStudent.Student);

//            RemoveUser(user);
//            RemoveStudent(student);
//        }
//    }
//}
