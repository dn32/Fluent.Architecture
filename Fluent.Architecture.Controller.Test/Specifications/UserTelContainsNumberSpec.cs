// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Fluent.Architecture.Controller.Test.Model;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Controller.Test.Specifications
{
    public class UserTelContainsNumberSpec : FluentSpecification<User>
    {
        private string _number;

        public UserTelContainsNumberSpec DefineParams(string number)
        {
            this._number = number;
            return this;
        }

        public override IQueryable<User> Where(IQueryable<User> query)
        {
            return query.Where(x => x.Tel.Contains(this._number));
        }

        public override IOrderedQueryable<User> Order(IQueryable<User> query)
        {
            return query.OrderBy(x => x.Name);
        }
    }
}