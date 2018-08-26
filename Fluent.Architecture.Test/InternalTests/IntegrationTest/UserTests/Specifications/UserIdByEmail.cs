using System;
using System.Linq;
using Fluent.Architecture.Service;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Specifications
{
    public class UserIdByEmail : FluentSelectSpecification<User, int>
    {
        private readonly string _email;

        public UserIdByEmail(TransactionalService service, string email) : base(service)
        {
            _email = email;
        }

        public override IQueryable<int> Spec(IQueryable<User> query)
        {
            return query.Where(x => x.Email.Equals(_email, StringComparison.CurrentCultureIgnoreCase)).Select(x => x.Id);
        }
    }
}