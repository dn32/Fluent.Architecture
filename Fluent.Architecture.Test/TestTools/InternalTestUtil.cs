// ReSharper disable CommentTypo

using Fluent.Architecture.Test.SupportElements;
using Fluent.Architecture.Test.SupportElements.Mock;
using Fluent.Architecture.Test.SupportElements.Model;

namespace Fluent.Architecture.Test.TestTools
{
    internal static class InternalTestUtil
    {
        public static User GetNewUser()
        {
            var rand = TestUtil.NextRandom();
            return new User
            {
                PersonType = ePersonType.User,
                Id = rand,
                UserName = $"maria {rand}",
                Name = $"maria {rand}",
                Email = $"test{rand}@mail.com",
                Password = $"test{rand}@mail.com",
                Tel = $"test{rand}@mail.com",
            };
        }

        public static Student GetNewStudent()
        {
            var rand = TestUtil.NextRandom();
            return new Student
            {
                Name = $"Name {rand}",
                Document = $"Document {rand}",
                Email = $"Email {rand}"
            };
        }
    }
}
