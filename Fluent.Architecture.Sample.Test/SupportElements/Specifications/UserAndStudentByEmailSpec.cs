// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Specifications;
using System;
using System.Linq;

namespace Fluent.Architecture.Sample.Test.SupportElements.Specifications
{
    public class UserAndStudentByEmailSpec : FluentSelectSpecification<User, UserStudent>
    {
        private string _email;

        public UserAndStudentByEmailSpec DefineParams(string email)
        {
            this._email = email;

            return this;
        }

        public override IQueryable<UserStudent> Where(IQueryable<User> query)
        {
            var students = this.Get<Student>();

            return query.Where(x => x.Email.Equals(this._email, StringComparison.CurrentCultureIgnoreCase))
                .Join(students, user => user.Email, student => student.Email, (user, student) => new { user, student })
                .Select(x => new UserStudent { User = x.user, Student = x.student });
        }

        public override IOrderedQueryable<UserStudent> Order(IQueryable<UserStudent> query)
        {
            return query.OrderBy(x => x.User.Name);
        }
    }
}