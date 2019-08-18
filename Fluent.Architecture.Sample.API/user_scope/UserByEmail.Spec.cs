using Fluent.Architecture.Specifications;
using System;
using System.Linq;

public class UserByEmailSpec : FluentSpecification<User>
{
    public string Email { get; set; }

    public UserByEmailSpec AddParameter(string email)
    {
        Email = email;
        return this;
    }

    public override IQueryable<User> Where(IQueryable<User> query)
    {
        return query.Where(x => x.Email.Equals(Email, StringComparison.InvariantCultureIgnoreCase));
    }

    public override IOrderedQueryable<User> Order(IQueryable<User> query)
    {
        return query.OrderBy(x => x.Name);
    }
}
