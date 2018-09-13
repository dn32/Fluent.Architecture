using System.Linq;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Sample.Test.SupportElements.Specifications
{
    public class CourseAllSpec : FluentSpecification<Course>
    {
        public CourseAllSpec(FluentController<Course> controller) : base(controller)
        {

        }

        public override IQueryable<Course> Where(IQueryable<Course> query)
        {
            return query.Where(x => true);
        }

        public override IOrderedQueryable<Course> Order(IQueryable<Course> query)
        {
            return query.OrderBy(x => x.Title);
        }
    }
}