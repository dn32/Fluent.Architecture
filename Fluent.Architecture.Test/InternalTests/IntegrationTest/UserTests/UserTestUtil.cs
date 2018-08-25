using System;
using System.Threading;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests
{
    public static class UserTestUtil
    {
        public static User GetNew()
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
    }
}
