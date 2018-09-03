using System;
using System.Linq;
using System.Linq.Expressions;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Sample.Test.SupportElements.Specifications
{
    public class StudentByEmailSpec : FluentSpecification<Student>
    {
        private readonly string _email;

        public StudentByEmailSpec(FluentController<Student> controller, string email) : base(controller)
        {
            this._email = email;
        }

        public StudentByEmailSpec(TransactionalService service, string email) : base(service)
        {
            this._email = email;
        }

        public override IQueryable<Student> Where(IQueryable<Student> query)
        {
            return query.Where(x => x.Email.Equals(this._email, StringComparison.CurrentCultureIgnoreCase));
        }

        public override Expression<Func<Student, object>> Order()
        {
            return x => x.Name;
        }
    }
}
