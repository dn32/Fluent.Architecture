using System;
using System.Linq;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Sample.Test.SupportElements.Specifications
{
    public class UserAndStudentByEmail : FluentSelectSpecification<User, UserStudent>
    {
        private readonly string _email;

        public UserAndStudentByEmail(FluentController<User> controller, string email) : base(controller)
        {
            this._email = email;
        }

        public override IQueryable<UserStudent> Where(IQueryable<User> query)
        {
            var students = this.Get<Student>();

            return query.Where(x => x.Email.Equals(this._email, StringComparison.CurrentCultureIgnoreCase))
                .Join(students, user => user.Email, student => student.Email, (user, student) => new { user, student })
                .Select(x => new UserStudent{User = x.user , Student = x.student });
        }
    }
}