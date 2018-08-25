using System;
using System.Linq;
using System.Linq.Expressions;
using Fluent.Architecture.Service;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest.StudentTests.Specifications
{
   public class StudentByTitleSpec: FluentSpecification<Student>
   {
       private readonly string _title;

        public StudentByTitleSpec(TransactionalService service, string title) : base(service)
        {
            _title = title;
        }

        public override IQueryable<Student> Spec(IQueryable<Student> query)
        {
            return query.Where(x => x.Name.Equals(_title, StringComparison.CurrentCultureIgnoreCase));
        }

        public override Expression<Func<Student, object>> Order()
        {
            return x => x.Name;
        }
    }
}
