using System;
using Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests.Enum;
using Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests.Models;

namespace Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests
{
    public static class UserTestUtil
    {
        public static User GetNewUser()
        {
            var rand = new Random().Next(1, int.MaxValue);
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
