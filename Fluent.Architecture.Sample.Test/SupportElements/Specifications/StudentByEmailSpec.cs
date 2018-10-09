using System;
using System.Data.Entity;
using System.Linq;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Sample.Test.SupportElements.Specifications
{
    public class StudentByEmailSpec : FluentSpecification<Student>
    {
        private readonly string _email;


        public StudentByEmailSpec(TransactionalService service, string email) : base(service)
        {
            this._email = email;
        }

        public override IQueryable<Student> Where(DbSet<Student> query)
        {
            return query.Where(x => x.Email.Equals(this._email, StringComparison.CurrentCultureIgnoreCase));
        }

        public override IOrderedQueryable<Student> Order(IQueryable<Student> query)
        {
            return query.OrderBy(x => x.Name);
        }
    }
}
