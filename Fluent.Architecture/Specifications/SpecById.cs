
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
    public class SpecById<T> : FluentSpecification<T> where T : FluentIdEntity
    {
        private int Id { get; set; }

        public SpecById(TransactionalService service, int id) : base(service)
        {
            Id = id;
        }

        public override IQueryable<T> Where(DbSet<T> query)
        {
            return query.Where(x => x.Id == Id);
        }

        public override IOrderedQueryable<T> Order(IQueryable<T> query)
        {
            return query.OrderBy(x => x.Id);
        }
    }
}
