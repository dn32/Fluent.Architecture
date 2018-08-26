using System;
using System.Linq;
using System.Linq.Expressions;
using Fluent.Architecture.Service;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.SupportElements.Model;

namespace Fluent.Architecture.Test.SupportElements.Specifications
{
    internal class UserByEmail : FluentSpecification<User>
    {
        private readonly string _email;

        public UserByEmail(TransactionalService service, string email) : base(service)
        {
            _email = email;
        }

        public override IQueryable<User> Spec(IQueryable<User> query)
        {
            return query.Where(x => x.Email.Equals(_email, StringComparison.CurrentCultureIgnoreCase));
        }

        public override Expression<Func<User, object>> Order()
        {
            return x => x.Name;
        }
    }
}