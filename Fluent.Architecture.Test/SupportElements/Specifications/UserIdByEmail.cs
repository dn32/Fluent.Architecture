using System;
using System.Linq;
using Fluent.Architecture.Service;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.SupportElements.Model;

namespace Fluent.Architecture.Test.SupportElements.Specifications
{
    internal class UserIdByEmail : FluentSelectSpecification<User, int>
    {
        private readonly string _email;

        public UserIdByEmail(TransactionalService service, string email) : base(service)
        {
            _email = email;
        }

        public override IQueryable<int> Spec(IQueryable<User> query)
        {
            return query.Where(x => x.Email.Equals(_email, StringComparison.CurrentCultureIgnoreCase)).Select(x => x.Id);
        }
    }
}