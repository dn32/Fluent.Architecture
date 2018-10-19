
#if NETCOREAPP2_1

using Microsoft.EntityFrameworkCore;

#else

using System.Data.Entity;

#endif

using Fluent.Architecture.Model;
using Fluent.Architecture.Services;
using System.Linq;

namespace Fluent.Architecture.Specifications
{
    public class AllSpec<T> : FluentSpecification<T> where T : FluentIdEntity
    {
        public AllSpec(TransactionalService service) : base(service)
        {
        }

        public override IQueryable<T> Where(DbSet<T> query)
        {
            return query;
        }

        public override IOrderedQueryable<T> Order(IQueryable<T> query)
        {
            return query.OrderBy(x => x.Id);
        }
    }
}
