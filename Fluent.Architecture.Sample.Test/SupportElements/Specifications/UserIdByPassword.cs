using System;
using System.Linq;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Sample.Test.SupportElements.Specifications
{
    public class UserIdByPassword : FluentSelectSpecification<User, int>
    {
        private readonly string _password;

        public UserIdByPassword(FluentController<User> controller, string password) : base(controller)
        {
            this._password = password;
        }

        public override IQueryable<int> Spec(IQueryable<User> query)
        {
            return query.Where(x => x.Password.Equals(this._password, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Id);
        }
    }
}