using System;
using System.Linq;
using System.Linq.Expressions;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Sample.Test.SupportElements.Specifications
{
    public class UserTelContainsNumber : FluentSpecification<User>
    {
        private readonly string _number;

        public UserTelContainsNumber(FluentController<User> controller, string number) : base(controller)
        {
            this._number = number;
        }

        public override IQueryable<User> Where(IQueryable<User> query)
        {
            return query.Where(x => x.Tel.Contains(this._number));
        }

        public override Expression<Func<User, object>> Order()
        {
            return x => x.Name;
        }
    }
}