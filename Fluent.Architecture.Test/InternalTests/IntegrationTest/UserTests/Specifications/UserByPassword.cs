using System;
using System.Linq;
using System.Linq.Expressions;
using Fluent.Architecture.Service;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Specifications
{
    public class UserByPassword : FluentSpecification<User>
    {
        private readonly string _password;

        public UserByPassword(TransactionalService service, string password) : base(service)
        {
            _password = password;
        }

        public override IQueryable<User> Spec(IQueryable<User> query)
        {
            return query.Where(x => x.Password.Equals(_password, StringComparison.InvariantCultureIgnoreCase));
        }

        public override Expression<Func<User, object>> Order()
        {
            return x => x.Name;
        }
    }
}