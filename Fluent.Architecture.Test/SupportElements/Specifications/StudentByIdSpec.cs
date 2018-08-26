using System;
using System.Linq;
using System.Linq.Expressions;
using Fluent.Architecture.Service;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.SupportElements.Model;

namespace Fluent.Architecture.Test.SupportElements.Specifications
{
    internal class StudentByTitleSpec: FluentSpecification<Student>
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
