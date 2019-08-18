// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Sample.Test.SupportElements.Services
{
    public class UserService : FluentService<User>
    {
        protected virtual FluentService<Student> StudentService => null;

        //public UserStudent GetUserByEmail(string email)
        //{
        //    var userSpec = CreateSpec<UserByEmailSpec>().DefineParams(email);
        //    var studentSpec = CreateSpec<StudentByEmailSpec>().DefineParams(email);
        //    //var studentSpec = new StudentByEmailSpec(this, email);

        //    var user = this.FirstOrDefault(userSpec);
        //    var student = this.StudentService.FirstOrDefault(studentSpec);
        //    var student2 = this.StudentService2.FirstOrDefault(studentSpec); // Para o teste de reutilização de serviço na injeção

        //    return new UserStudent
        //    {
        //        User = user,
        //        Student = student
        //    };
        //}
    }
}