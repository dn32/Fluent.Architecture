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
    public class UserByEmailSpec : FluentSpecification<User>
    {
        private string _email;

        public UserByEmailSpec DefineParams(string email)
        {
            this._email = email;
            return this;
        }

        public override IQueryable<User> Where(IQueryable<User> query)
        {
            return query.Where(x => x.Email.Equals(this._email, StringComparison.CurrentCultureIgnoreCase));
        }

        public override IOrderedQueryable<User> Order(IQueryable<User> query)
        {
            return query.OrderBy(x => x.Name);
        }
    }
}