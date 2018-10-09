using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Sample.Test.SupportElements.Specifications
{
    public class UserByEmail : FluentSpecification<User>
    {
        private readonly string _email;

        public UserByEmail(FluentController<User> controller, string email) : base(controller)
        {
            this._email = email;
        }

        public UserByEmail(TransactionalService service, string email) : base(service)
        {
            this._email = email;
        }

        public override IQueryable<User> Where(DbSet<User> query)
        {
            return query.Where(x => x.Email.Equals(this._email, StringComparison.CurrentCultureIgnoreCase));
        }

        public override IOrderedQueryable<User> Order(IQueryable<User> query)
        {
            return query.OrderBy(x => x.Name);
        }
    }
}