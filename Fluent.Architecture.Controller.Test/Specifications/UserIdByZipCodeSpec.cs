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
    public class UserIdByZipCodeSpec : FluentSelectSpecification<User, int>
    {
        private long _zipCode;

        public UserIdByZipCodeSpec DefineParams(long zipCode)
        {
            _zipCode = zipCode;
            return this;
        }
      
        public override IQueryable<int> Where(IQueryable<User> query)
        {
            return query.Where(x => x.ZipCode ==_zipCode).Select(x => x.Id);
        }

        public override IOrderedQueryable<int> Order(IQueryable<int> query)
        {
            return query.OrderBy(x => x);
        }
    }
}