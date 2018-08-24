using System;
using System.Threading;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Enum;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Models;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests
{
    public static class UserTestUtil
    {
        public static int Count { get; set; }

        public static User GetNewUser()
        {
            Count++;
            var rand = new Random().Next(Count, 65000) + Count;
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
