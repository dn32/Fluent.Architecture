using System;
using System.Linq;
using System.Linq.Expressions;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Sample.Test.SupportElements.Specifications
{
    public class StudentByNameSpec: FluentSpecification<Student>
   {
       private readonly string _name;

        public StudentByNameSpec(FluentController<Student> controller, string name) : base(controller)
        {
            this._name = name;
        }

        public override IQueryable<Student> Spec(IQueryable<Student> query)
        {
            return query.Where(x => x.Name.Equals(this._name, StringComparison.CurrentCultureIgnoreCase));
        }

        public override Expression<Func<Student, object>> Order()
        {
            return x => x.Name;
        }
    }
}
