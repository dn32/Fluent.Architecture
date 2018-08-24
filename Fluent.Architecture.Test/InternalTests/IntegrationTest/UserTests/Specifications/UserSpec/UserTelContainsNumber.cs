using System.Linq;
using Fluent.Architecture.Service;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Models;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Specifications.UserSpec
{
    public class UserTelContainsNumber : FluentSpecification<User>
    {
        private readonly string _number;

        public UserTelContainsNumber(TransactionalService service, string number) : base(service)
        {
            _number = number;
        }

        public override IQueryable<User> Spec(IQueryable<User> query)
        {
            return query.Where(x => x.Tel.Contains(_number));
        }
    }

    public class UserAll : FluentSpecification<User>
    {
        public UserAll(TransactionalService service) : base(service)
        {
        }

        public override IQueryable<User> Spec(IQueryable<User> query)
        {
            return query.Where(x => true);
        }
    }
}