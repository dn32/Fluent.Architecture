using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Sample.Test.SupportElements.Specifications;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Sample.Test.SupportElements.Services
{
    public class UserService : FluentService<User>
    {
        protected virtual FluentService<Student> StudentService => null;

        protected virtual FluentService<Student> StudentService2 => null;

        public UserStudent GetUserByEmail(string email)
        {
            var userSpec = new UserByEmail(this, email);
            var studentSpec = new StudentByEmailSpec(this, email);

            var user = this.SpecOne(userSpec);
            var student = this.StudentService.SpecOne(studentSpec);
            var student2 = this.StudentService2.SpecOne(studentSpec); // Para o teste de reutilização de serviço na injeção

            return new UserStudent
            {
                User = user,
                Student = student
            };
        }
    }
}