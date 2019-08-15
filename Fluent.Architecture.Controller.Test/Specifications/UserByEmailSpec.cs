// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Fluent.Architecture.Controller.Test.Model;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Controller.Test.Specifications
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