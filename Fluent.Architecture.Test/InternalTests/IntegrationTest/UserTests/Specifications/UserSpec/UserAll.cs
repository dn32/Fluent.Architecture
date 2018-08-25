using System;
using System.Linq;
using System.Linq.Expressions;
using Fluent.Architecture.Service;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Models;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Specifications.UserSpec
{
    public class UserAll : FluentSpecification<User>
    {
        public UserAll(TransactionalService service) : base(service)
        {
        }

        public override IQueryable<User> Spec(IQueryable<User> query)
        {
            return query.Where(x => true);
        }


        public override Expression<Func<User, object>> Order()
        {
            return x => x.Name;
        }
    }
}