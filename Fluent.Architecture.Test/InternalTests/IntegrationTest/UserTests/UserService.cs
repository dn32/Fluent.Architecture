using Fluent.Architecture.Service;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.StudentTests;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.StudentTests.Specifications;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Specifications;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests
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