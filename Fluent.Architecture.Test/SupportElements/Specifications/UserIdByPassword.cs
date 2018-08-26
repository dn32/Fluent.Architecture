using System;
using System.Linq;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.SupportElements.Model;

namespace Fluent.Architecture.Test.SupportElements.Specifications
{
    public class UserIdByPassword : FluentSelectSpecification<User, int>
    {
        private readonly string _password;

        public UserIdByPassword(FluentController<User> controller, string password) : base(controller)
        {
            _password = password;
        }

        public override IQueryable<int> Spec(IQueryable<User> query)
        {
            return query.Where(x => x.Password.Equals(_password, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Id);
        }
    }
}