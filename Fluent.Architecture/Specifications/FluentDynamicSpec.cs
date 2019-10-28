using Fluent.Architecture.Core.Extensions;
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Specifications;
using System.Linq;

namespace Fluent.Architecture.Core.Specifications
{
    public class FluentDynamicSpec<T> : FluentSpecification<T> where T : FluentEntity
    {
        public string[] Fields { get; set; }

        public bool IsList { get; set; }

        public FluentDynamicSpec<T> SetParameters(string[] fields, bool isList)
        {
            Fields = fields;
            IsList = isList;
            return this;
        }

        public override IQueryable<T> Where(IQueryable<T> query)
        {
            query = query.GetInclusions(IsList);
            return query.FluentDynamicProjectTo(Service, Fields);
        }

        public override IOrderedQueryable<T> Order(IQueryable<T> query)
        {
            return query.FluentDynamicProjectToOrder(Service);
        }
    }
}
