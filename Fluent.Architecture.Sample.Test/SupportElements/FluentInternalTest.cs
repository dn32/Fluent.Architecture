// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Sample.Test.SupportElements.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Sample.Test.SupportElements.Services;
using Fluent.Architecture.Test;
using Fluent.Architecture.Test.Mock;

namespace Fluent.Architecture.Sample.Test.SupportElements
{
    internal class FluentInternalTest
    {
        public UserController UserControllerInstance { get; set; }

        public StudentController StudentControllerInstance { get; set; }

        public ReportController ReportControllerInstance { get; set; }

        internal void RemoveUser(User user)
        {
            TestUtil.Execute(UserControllerInstance, (UserController controller) => controller.Remove(user));
        }

        internal void RemoveStudent(Student student)
        {
            TestUtil.Execute(StudentControllerInstance, (StudentController controller) => controller.Remove(student));
        }

        internal User AddUser(User user)
        {
            return TestUtil.ExecuteAndResult<User, UserController>(UserControllerInstance, (UserController controller) => controller.Add(user));
        }

        internal Student AddStudent(Student student)
        {
            return TestUtil.ExecuteAndResult<Student, StudentController>(StudentControllerInstance, (StudentController controller) => controller.Add(student));
        }

        internal User FindUser(User user)
        {
            return TestUtil.ExecuteAndResult<User, UserController>(UserControllerInstance, (UserController controller) => controller.Find(user));
        }

        internal void RemoveRangeUser(User[] users)
        {
            TestUtil.Execute(UserControllerInstance, (UserController controller) => controller.RemoveRange(users));
        }

        internal User UpdateUser(User user)
        {
            return TestUtil.ExecuteAndResult<User, UserController>(UserControllerInstance, (UserController controller) => controller.Update(user));
        }

        public FluentInternalTest()
        {
            Fluent.Architecture.Test.Setup.Initialize(null);
            Fluent.Architecture.Setup.SetGlobalizationServiceType<FluentGlobalizationService>(null);

            this.UserControllerInstance = MockUtil.GetMockController<UserController>();
            this.StudentControllerInstance = MockUtil.GetMockController<StudentController>();
            //  this.CourseControllerInstance = MockUtil.GetMockController<CourseController>();
            this.ReportControllerInstance = MockUtil.GetMockController<ReportController>();
        }
    }
}