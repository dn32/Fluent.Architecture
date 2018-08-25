using System;
using System.Threading;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Enum;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Models;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests
{
    public static class UserTestUtil
    {
        private static readonly Random Random = new Random();

        private static readonly object SyncLock = new object();

        public static User GetNewUser()
        {
            var rand = NextRandom();
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


        public static int NextRandom()
        {
            lock (SyncLock)
            {
                return Random.Next(1, 65000);
            }
        }
    }
}
