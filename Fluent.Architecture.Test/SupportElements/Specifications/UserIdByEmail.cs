using System;
using System.Linq;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.SupportElements.Model;

namespace Fluent.Architecture.Test.SupportElements.Specifications
{
    public class UserIdByEmail : FluentSelectSpecification<User, int>
    {
        private readonly string _email;

        public UserIdByEmail(FluentController<User> controller, string email) : base(controller)
        {
            _email = email;
        }

        public override IQueryable<int> Spec(IQueryable<User> query)
        {
            return query.Where(x => x.Email.Equals(_email, StringComparison.CurrentCultureIgnoreCase)).Select(x => x.Id);
        }
    }
}