using Fluent.Architecture.Services;
using Fluent.Architecture.Test.SupportElements.Model;
using Fluent.Architecture.Test.SupportElements.Specifications;

namespace Fluent.Architecture.Test.SupportElements.Services
{
    public class UserService : FluentService<User>
    {
        protected virtual FluentService<Student> StudentService => null;

        public UserStudent GetUserByEmail(string email)
        {
            var userSpec = new UserByEmail(this, email);
            var studentSpec = new StudentByEmailSpec(this, email);

            var user = SpecOne(userSpec);
            var student = StudentService.SpecOne(studentSpec);

            return new UserStudent
            {
                User = user,
                Student = student
            };
        }
    }
}