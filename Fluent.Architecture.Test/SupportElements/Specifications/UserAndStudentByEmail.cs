using System;
using System.Linq;
using Fluent.Architecture.Service;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.SupportElements.Model;

namespace Fluent.Architecture.Test.SupportElements.Specifications
{
    internal class UserAndStudentByEmail : FluentSelectSpecification<User, UserStudent>
    {
        private readonly string _email;

        public UserAndStudentByEmail(TransactionalService service, string email) : base(service)
        {
            _email = email;
        }

        public override IQueryable<UserStudent> Spec(IQueryable<User> query)
        {
            var students = Get<Student>();

            return query.Where(x => x.Email.Equals(_email, StringComparison.CurrentCultureIgnoreCase))
                .Join(students, user => user.Id, student => student.Id, (user, student) => new { user, student })
                .Select(x => new UserStudent{User = x.user , Student = x.student });
        }
    }
}