using System.Linq;
using Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests.Models;
using Fluent.Architecture.Service;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests.Specifications.UserSpec
{
    public class UserByEmail : SpecificationIQueryableSpec<User>
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