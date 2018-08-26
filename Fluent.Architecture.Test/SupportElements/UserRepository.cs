using System.Linq;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Test.SupportElements.Model;

namespace Fluent.Architecture.Test.SupportElements
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
