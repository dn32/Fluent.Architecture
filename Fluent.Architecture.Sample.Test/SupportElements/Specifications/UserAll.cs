using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Sample.Test.SupportElements.Specifications
{
    public class UserAll : FluentSpecification<User>
    {
        public UserAll(TransactionalService service) : base(service)
        {
        }

        public override IQueryable<User> Where(DbSet<User> query)
        {
            return query.Where(x => true);
        }

        public override IOrderedQueryable<User> Order(IQueryable<User> query)
        {
            return query.OrderBy(x => x.Name);
        }
    }
}