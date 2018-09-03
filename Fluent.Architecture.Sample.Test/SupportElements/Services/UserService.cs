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

            var user = this.FirstOrDefault(userSpec);
            var student = this.StudentService.FirstOrDefault(studentSpec);
            var student2 = this.StudentService2.FirstOrDefault(studentSpec); // Para o teste de reutilização de serviço na injeção

            return new UserStudent
            {
                User = user,
                Student = student
            };
        }
    }
}