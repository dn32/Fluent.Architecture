// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Sample.Test.SupportElements.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
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
            TestUtil.Execute<UserController, bool>(UserControllerInstance, (UserController controller) => controller.Remove(user));
        }

        internal void RemoveStudent(Student student)
        {
            TestUtil.Execute<StudentController, bool>(StudentControllerInstance, (StudentController controller) => controller.Remove(student));
        }

        internal void AddUser(User user)
        {
            // O elemento salvo não retorna um ID válido, pois somente após a serialização do retorno é que a transação é finalizada
            // Deve-se consultar o elemento para se obter o id
            TestUtil.Execute<UserController, User>(UserControllerInstance, (UserController controller) => controller.Add(user));
        }

        internal void AddRangeUsers(User[] users)
        {
            // O elemento salvo não retorna um ID válido, pois somente após a serialização do retorno é que a transação é finalizada
            // Deve-se consultar o elemento para se obter o id
            TestUtil.Execute<UserController, User[]>(UserControllerInstance, (UserController controller) => controller.AddRange(users));
        }

        internal void AddStudent(Student student)
        {
            // O elemento salvo não retorna um ID válido, pois somente após a serialização do retorno é que a transação é finalizada
            // Deve-se consultar o elemento para se obter o id
            TestUtil.Execute<StudentController, Student>(StudentControllerInstance, (StudentController controller) => controller.Add(student));
        }

        internal User FindUser(User user)
        {
            return TestUtil.Execute<UserController, User>(UserControllerInstance, (UserController controller) => controller.Find(user));
        }

        internal Student FindStudent(Student student)
        {
            return TestUtil.Execute<StudentController, Student>(StudentControllerInstance, (StudentController controller) => controller.Find(student));
        }

        internal void RemoveRangeUser(User[] users)
        {
            TestUtil.Execute<UserController, User[]>(UserControllerInstance, (UserController controller) => controller.RemoveRange(users));
        }

        internal bool UpdateUser(User user)
        {
            return TestUtil.Execute<UserController, bool>(UserControllerInstance, (UserController controller) => controller.Update(user));
        }

        public FluentInternalTest()
        {
            Fluent.Architecture.Test.Setup.Initialize(null);

            this.UserControllerInstance = MockUtil.GetMockController<UserController>();
            this.StudentControllerInstance = MockUtil.GetMockController<StudentController>();
            //  this.CourseControllerInstance = MockUtil.GetMockController<CourseController>();
            this.ReportControllerInstance = MockUtil.GetMockController<ReportController>();
        }
    }
}