using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Fluent.Architecture.Service;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest.StudentTests.Specifications
{
    public class StudentByEmailSpec : FluentSpecification<Student>
    {
        private readonly string _email;

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
