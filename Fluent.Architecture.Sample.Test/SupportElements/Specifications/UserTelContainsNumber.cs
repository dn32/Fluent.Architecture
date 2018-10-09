using System;
using System.Data.Entity;
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

        public override IQueryable<User> Where(DbSet<User> query)
        {
            return query.Where(x => x.Tel.Contains(this._number));
        }

        public override IOrderedQueryable<User> Order(IQueryable<User> query)
        {
            return query.OrderBy(x => x.Name);
        }
    }
}