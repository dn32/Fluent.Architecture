using System;
using System.Linq;
using System.Linq.Expressions;
using Fluent.Architecture.Service;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.SupportElements.Model;

namespace Fluent.Architecture.Test.SupportElements.Specifications
{
    internal class UserTelContainsNumber : FluentSpecification<User>
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

        public override Expression<Func<User, object>> Order()
        {
            return x => x.Name;
        }
    }
}