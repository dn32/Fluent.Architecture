using System;
using System.Linq;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Sample.Test.SupportElements.Specifications
{
    public class UserIdByEmail : FluentSelectSpecification<User, int>
    {
        private readonly string _email;

        public UserIdByEmail(FluentController<User> controller, string email) : base(controller)
        {
            this._email = email;
        }

        public override IQueryable<int> Where(IQueryable<User> query)
        {
            return query.Where(x => x.Email.Equals(this._email, StringComparison.CurrentCultureIgnoreCase)).Select(x => x.Id);
        }

        public override IOrderedQueryable<int> Order(IQueryable<int> query)
        {
            return query.OrderBy(x => x);
        }
    }
}