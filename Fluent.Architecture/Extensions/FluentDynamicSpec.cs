using Fluent.Architecture.Core.Extensions;
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Specifications;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Fluent.Architecture.Core.Specifications
{
    public class FluentDynamicSpec<T> : FluentSelectSpecification<T, object> where T : FluentEntity
    {
        public string[] Fields { get; set; }

        public bool IsList { get; set; }

        public FluentDynamicSpec<T> SetParameters(string[] fields, bool isList)
        {
            Fields = fields;
            IsList = isList;
            return this;
        }

        public override IQueryable<object> Where(IQueryable<T> query)
        {
            query = query.GetInclusions(IsList);
            return query.FluentDynamicProjectTo(Fields);
        }

        public override IOrderedQueryable<object> Order(IQueryable<object> query)
        {
            return query.FluentDynamicProjectToOrder(Service);
        }
    }
}
