using System;
using System.Threading;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Enum;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Models;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests
{
    public static class UserTestUtil
    {
        public static int Count { get; set; }

        public static object ObjectLock = new object();

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
            lock (ObjectLock)
            {
                return new Random().Next(Count, 65000) + Count++;
            }
        }
    }
}
