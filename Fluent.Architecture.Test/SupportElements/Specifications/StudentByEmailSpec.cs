using System;
using System.Linq;
using System.Linq.Expressions;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.SupportElements.Model;

namespace Fluent.Architecture.Test.SupportElements.Specifications
{
    public class StudentByEmailSpec : FluentSpecification<Student>
    {
        private readonly string _email;

        public StudentByEmailSpec(FluentController<Student> controller, string email) : base(controller)
        {
            _email = email;
        }

        public StudentByEmailSpec(TransactionalService service, string email) : base(service)
        {
            _email = email;
        }

        public override IQueryable<Student> Spec(IQueryable<Student> query)
        {
            return query.Where(x => x.Email.Equals(_email, StringComparison.CurrentCultureIgnoreCase));
        }

        public override Expression<Func<Student, object>> Order()
        {
            return x => x.Name;
        }
    }
}
