using Fluent.Architecture.Core.Extensions;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Specifications;
using System.Linq;

namespace Fluent.Architecture.Core.Specifications
{
    public class FluentAllSpec<T> : FluentSpecification<T> where T : FluentEntity
    {
        public bool IsList { get; set; }

        public FluentAllSpec<T> SetParameter(bool isList)
        {
            IsList = isList;
            return this;
        }

        public override IQueryable<T> Where(IQueryable<T> query)
        {
           return query.GetInclusions(IsList);
        }

        public override IOrderedQueryable<T> Order(IQueryable<T> query)
        {
            return query.OrderBy(x => x);
        }
    }
}
