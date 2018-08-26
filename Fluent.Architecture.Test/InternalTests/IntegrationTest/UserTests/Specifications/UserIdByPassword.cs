using System;
using System.Linq;
using Fluent.Architecture.Service;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Specifications
{
    public class UserIdByPassword : FluentSelectSpecification<User, int>
    {
        private readonly string _password;

        public UserIdByPassword(TransactionalService service, string password) : base(service)
        {
            _password = password;
        }

        public override IQueryable<int> Spec(IQueryable<User> query)
        {
            return query.Where(x => x.Password.Equals(_password, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Id);
        }
    }
}