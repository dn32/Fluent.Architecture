using System.Linq;
using Fluent.Architecture.Service;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Models;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Specifications.UserSpec
{
    public class UserByEmail : FluentSpecification<User>
    {
        private readonly string _email;

        public UserByEmail(TransactionalService service, string email) : base(service)
        {
            _email = email;
        }

        public override IQueryable<User> Spec(IQueryable<User> query)
        {
            return query.Where(x => x.Email == _email);
        }
    }
}