using System;
using System.Linq;
using System.Linq.Expressions;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Sample.Test.SupportElements.Specifications
{
    public class UserAll : FluentSpecification<User>
    {
        public UserAll(TransactionalService service) : base(service)
        {
        }

        public override IQueryable<User> Where(IQueryable<User> query)
        {
            return query.Where(x => true);
        }

        public override Expression<Func<User, object>> Order()
        {
            return x => x.Name;
        }
    }
}