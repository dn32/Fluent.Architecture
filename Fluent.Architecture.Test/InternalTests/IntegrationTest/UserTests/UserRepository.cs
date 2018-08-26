using System.Linq;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Repository;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests
{
    public class UserRepository:FluentRepository<User>
    {
        [Propagate]
        public User FindById(int id)
        {
            return Query.FirstOrDefault(x => x.Id.Equals(id));
        }
    }
}
