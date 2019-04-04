using Fluent.Architecture.Model;
using System.Linq;

namespace Fluent.Architecture.Specifications
{
    public class SpecFindById<T> : FluentSpecification<T> where T : FluentIdEntity
    {
        public int Id { get; set; }

        public SpecFindById<T> DefineParameters(int id)
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
