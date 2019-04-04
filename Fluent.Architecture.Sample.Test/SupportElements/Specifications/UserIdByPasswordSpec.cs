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
    public class UserIdByPasswordSpec : FluentSelectSpecification<User, int>
    {
        private string _password;

        public UserIdByPasswordSpec DefineParams(string password)
        {
            this._password = password;
            return this;
        }
      
        public override IQueryable<int> Where(IQueryable<User> query)
        {
            return query.Where(x => x.Password.Equals(this._password, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Id);
        }

        public override IOrderedQueryable<int> Order(IQueryable<int> query)
        {
            return query.OrderBy(x => x);
        }
    }
}