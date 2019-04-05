
#if NETCOREAPP2_1

using Microsoft.EntityFrameworkCore;

#else

using System.Data.Entity;

#endif

using Fluent.Architecture.Model;
using System.Linq;

namespace Fluent.Architecture.Specifications
{
    public class SpecById<T> : FluentSpecification<T> where T : FluentIdEntity
    {
        private int Id { get; set; }

        public SpecById<T> SetParameter(int id)
        {
            Id = id;
            return this;
        }

        public override IQueryable<T> Where(IQueryable<T> query)
        {
            return query.Where(x => x.Id == Id);
        }

        public override IOrderedQueryable<T> Order(IQueryable<T> query)
        {
            return query.OrderBy(x => x.Id);
        }
    }
}
