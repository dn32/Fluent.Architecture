using System;
using System.Linq;
using System.Linq.Expressions;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Sample.Test.SupportElements.Specifications
{
    public class UserByPassword : FluentSpecification<User>
    {
        private readonly string _password;

        public UserByPassword(FluentController<User> controller, string password) : base(controller)
        {
            this._password = password;
        }

        public override IQueryable<User> Spec(IQueryable<User> query)
        {
            return query.Where(x => x.Password.Equals(this._password, StringComparison.InvariantCultureIgnoreCase));
        }

        public override Expression<Func<User, object>> Order()
        {
            return x => x.Name;
        }
    }
}