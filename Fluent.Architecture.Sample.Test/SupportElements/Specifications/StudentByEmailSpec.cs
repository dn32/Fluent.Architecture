// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Sample.Test.SupportElements.Specifications
{
    public class StudentByEmailSpec : FluentSpecification<Student>
    {
        private string _email;

        public StudentByEmailSpec DefineParams(string email)
        {
            this._email = email;
            return this;
        }

        public override IQueryable<Student> Where(IQueryable<Student> query)
        {
            return query.Where(x => x.Email.Equals(this._email, StringComparison.CurrentCultureIgnoreCase));
        }

        public override IOrderedQueryable<Student> Order(IQueryable<Student> query)
        {
            return query.OrderBy(x => x.Name);
        }
    }
}
