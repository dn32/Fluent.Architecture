using System.Linq;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Test.SupportElements.Model;

namespace Fluent.Architecture.Test.SupportElements
{
    public class UserRepository : FluentRepository<User>
    {
        [Propagate]
        public User FindById(int id)
        {
            return Query.FirstOrDefault(x => x.Id.Equals(id));
        }



        // ======================= PROPAGATION =========================


        [Propagate]
        public User PropagateMethodTestA()
        {
            return new User { Id = 1 };
        }

        [Propagate]
        public User PropagateMethodTestB(int id, string name)
        {
            return new User { Id = id, Name = name };
        }

        [Propagate]
        public Student PropagateMethodTestC(int id, string name)
        {
            return new Student { Id = id, Name = name };
        }

        [Propagate]
        public User PropagateMethodTestD(int id)
        {
            return new User { Id = id };
        }

        [Propagate]
        public Student PropagateMethodTestE(int id)
        {
            return new Student { Id = id };
        }

        //===========================

        [Propagate]
        public User PropagateTestF()
        {
            return new User { Id = 1 };
        }

        [Propagate]
        public User PropagateTestG(int id, string name)
        {
            return new User { Id = id, Name = name };
        }

        [Propagate]
        public Student PropagateTestH(int id, string name)
        {
            return new Student { Id = id, Name = name };
        }

        public Student PropagateTestI(int id)
        {
            return new Student { Id = id };
        }

        [Propagate]
        public User PropagateTestJ(int id)
        {
            return new User { Id = id };
        }
    }
}
