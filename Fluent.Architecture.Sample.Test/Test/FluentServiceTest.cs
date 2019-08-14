// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Claims;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Sample.Test.SupportElements;
using Fluent.Architecture.Sample.Test.SupportElements.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Sample.Test.TestTools;
using Fluent.Architecture.Test;
using NUnit.Framework;

namespace Fluent.Architecture.Sample.Test.Test
{
    [TestFixture]
    [ComVisible(true)]
    internal class FluentServiceTest : FluentInternalTest
    {
        [Theory]
        [TestCase(1, true)]
        [TestCase(1, false)]
        [TestCase(0, true)]
        [TestCase(0, false)]
        public void CountSuccessTest(int expectedCount, bool userSelectSpec)
        {
            var user = InternalTestUtil.GetNewUser();
            var passwordForFind = expectedCount == 0 ? user.Password + "xpto" : user.Password;

            //Add
            AddUser(user);

            var count = 0;
            if (userSelectSpec)
            {
                count = TestUtil.ExecuteAndResult<int, UserController>(UserControllerInstance, (UserController controller) => controller.CountIdByPassword(passwordForFind));
            }
            else
            {
                count = TestUtil.ExecuteAndResult<int, UserController>(UserControllerInstance, (UserController controller) => controller.CountByPassword(passwordForFind));
            }

            Assert.AreEqual(expectedCount, count);

            //Remove
            RemoveUser(user);
        }

        [Theory]
        [TestCase(true)]
        [TestCase(false)]
        public void FindOneTest(bool success)
        {
            var user = InternalTestUtil.GetNewUser();
            var email = success ? user.Email : user.Email + "xxy";

            //Add
            AddUser(user);

            user = TestUtil.ExecuteAndResult<User, UserController>(UserControllerInstance, (UserController controller) => controller.Find(user));
            var userFound = TestUtil.Execute(UserControllerInstance, (UserController controller) => controller.SpecOne(email));

            if (success)
            {
                Assert.NotNull(userFound);
                Assert.AreEqual(user.GetAllDataOfObject(), userFound.GetAllDataOfObject());
            }
            else
            {
                Assert.Null(userFound);
            }

            //Remove
            RemoveUser(user);
        }

        [Test]
        public void FindSelectSpecTest()
        {
            var user1 = InternalTestUtil.GetNewUser();
            var user2 = InternalTestUtil.GetNewUser();
            var password = $"{TestUtil.NextRandom()}{TestUtil.NextRandom()}{TestUtil.NextRandom()}";
            user1.Password = password;
            user2.Password = password;

            //Add
            AddUser(user1);
            AddUser(user2);

            //List
            var userIds = TestUtil.ExecuteAndResult<List<int>, UserController>(UserControllerInstance, (UserController controller) => controller.ListByIdPassword(password));

            Assert.NotNull(userIds);
            Assert.IsNotEmpty(userIds);
            Assert.AreEqual(2, userIds.Count);

            //Remove
            RemoveUser(user1);
            RemoveUser(user2);
        }

        [Test]
        public void RemoveRangeByEntitiesTest()
        {
            var users = new User[] { InternalTestUtil.GetNewUser(), InternalTestUtil.GetNewUser() };

            //Add
            AddRange(users);

            //Remove
            TestUtil.Execute(this.UserControllerInstance, (UserController controller) => controller.RemoveRange(users));

            var user1 = TestUtil.ExecuteAndResult<User, UserController>(UserControllerInstance, (UserController controller) => controller.Find(users.First()));
            var user2 = TestUtil.ExecuteAndResult<User, UserController>(UserControllerInstance, (UserController controller) => controller.Find(users.Last()));

            Assert.Null(user1);
            Assert.Null(user2);
        }

        private User[] AddRange(User[] users)
        {
            return TestUtil.ExecuteAndResult<User[], UserController>(UserControllerInstance, (UserController controller) => controller.AddRange(users));
        }

        [Test]
        public void SessionRequestIdTest()
        {
            object Method(UserController controller)
            {
                var id = controller?.GetType().GetProperty("SessionRequestId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(controller);
                if (id != null)
                {
                    return (Guid)id;
                }

                return default(Guid);
            }

            var sessionId = TestUtil.ExecuteAndResult<object, UserController>(this.UserControllerInstance, Method);

            Assert.AreNotEqual(Guid.Empty, sessionId);
        }

        [Test]
        public void UserTest()
        {
            object Method(UserController controller)
            {
                return controller?.GetType().GetProperty("ServiceUser", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(controller) as ClaimsPrincipal;
            }

            var serviceUser = TestUtil.Execute(UserControllerInstance, Method);
            Assert.NotNull(serviceUser);
        }

        [Test]
        public void HttpContextTest()
        {
            var httpContextBase = TestUtil.Execute(UserControllerInstance, (BaseController controller) => controller.HttpContext);
            Assert.NotNull(httpContextBase);
        }

        [Test]
        public void ServiceHttpContextTest()
        {
            object Method(UserController controller)
            {
                return controller?.GetType().GetProperty("ServiceHttpContext", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(controller);
            }

            var serviceUser = TestUtil.Execute(UserControllerInstance, Method);
            Assert.NotNull(serviceUser);
            Assert.NotNull(UserControllerInstance.User);
        }

        [Test]
        public void CountTest()
        {
            var user1 = InternalTestUtil.GetNewUser();

            //Add
            AddUser(user1);

            var count = TestUtil.ExecuteAndResult<int, UserController>(UserControllerInstance, (UserController controller) => controller.Count());
            FluentAssert.IsNotNullOrEmpty(count);

            //Remove
            RemoveUser(user1);
        }

        [Test]
        public void FirstOrDefaultTest()
        {
            //Add
            var user1 = InternalTestUtil.GetNewUser();
            AddUser(user1);

            var user = TestUtil.ExecuteAndResult<User, UserController>(UserControllerInstance, (UserController controller) => controller.FirstOrDefault());
            Assert.NotNull(user);

            //Remove
            RemoveUser(user1);
        }

        [Test]
        public void RemoveRangeBySpecTest()
        {
            var users = new User[] { InternalTestUtil.GetNewUser(), InternalTestUtil.GetNewUser() };
            var password = $"{TestUtil.NextRandom()}{TestUtil.NextRandom()}{TestUtil.NextRandom()}";
            users[0].Password = password;
            users[1].Password = password;

            //Add
            AddRange(users);

            //Remove
            RemoveRangeUser(users);

            var user1 = FindUser(users.First());
            var user2 = FindUser(users.Last());

            Assert.Null(user1);
            Assert.Null(user2);
        }

        [Theory]
        [TestCase(true)]
        [TestCase(false)]
        public void FindSelectSpecOneTest(bool success)
        {
            var user = InternalTestUtil.GetNewUser();

            //Add
            AddUser(user);
            var userId = TestUtil.ExecuteAndResult<int, UserController>(UserControllerInstance, (UserController controller) => controller.SpecOneInt(success ? user.Email : user.Email + "xxy"));

            if (success)
            {
                Assert.AreNotEqual(0, userId);
            }
            else
            {
                Assert.AreEqual(0, userId);
            }

            //Remove
            RemoveUser(user);
        }

        [Theory]
        [TestCase("62", 1)]
        [TestCase("63", 2)]
        [TestCase("64", 0)]
        public void CountTest(string number, int count)
        {
            var user1 = InternalTestUtil.GetNewUser();
            user1.Tel = "00000000000000";

            var users = new List<User> { user1 };

            number += TestUtil.NextRandom();

            for (var i = 0; i < count; i++)
            {
                var user = InternalTestUtil.GetNewUser();
                user.Tel = number + TestUtil.NextRandom();
                users.Add(user);
            }

            User[] usersArray = users.ToArray();

            //Add
            AddRange(usersArray);

            //Test Count
            var countFound = TestUtil.ExecuteAndResult<int, UserController>(UserControllerInstance, (UserController controller) => controller.CountByTelNumber(number));
            Assert.AreEqual(count, countFound);

            //Test SpecSelect
            var usersReturn = TestUtil.ExecuteAndResult<List<User>, UserController>(this.UserControllerInstance, (UserController controller) => controller.ListByTelNumber(number));
            Assert.NotNull(usersReturn);
            Assert.AreEqual(count, usersReturn.Count);

            //Remove
            RemoveRangeUser(usersArray);
        }

        [Theory]
        [TestCase(1, 10)]
        [TestCase(0, 10)]
        [TestCase(5, 7)]
        [TestCase(11, 10)]
        [TestCase(15, 10)]
        public void PaginationTest(int currentPage, int itemsPerPage)
        {
            var users = new List<User>();
            var telNumber = $"{TestUtil.NextRandom()}{TestUtil.NextRandom()}{TestUtil.NextRandom()}";
            for (var i = 1; i <= 103; i++)
            {
                var user = InternalTestUtil.GetNewUser();
                user.Tel = telNumber;
                user.Name = i.ToString("D5") + user.Name;
                user.Password = i.ToString("D5");
                users.Add(user);
            }

            currentPage = currentPage == 0 ? 1 : currentPage;

            var pages = users.Count / itemsPerPage;
            var expectedCount = itemsPerPage;

            if (users.Count % itemsPerPage > 0)
            {
                pages++;
            }

            if (currentPage == pages)
            {
                expectedCount = users.Count % itemsPerPage;
            }

            if (currentPage > pages)
            {
                expectedCount = 0;
            }


            User[] usersArray = users.ToArray();

            //Add
            AddRange(usersArray);

            //Test Count
            var countFound = TestUtil.ExecuteAndResult<int, UserController>(UserControllerInstance, (UserController controller) => controller.CountByTelNumber(telNumber));
            Assert.AreEqual(users.Count, countFound);

            var pagination = new FluentPagination(currentPage, false, itemsPerPage);

            //SpecSelect
            var found = TestUtil.ExecuteAndResult<List<User>, UserController>(UserControllerInstance, (UserController controller) => controller.ListByTelNumber(telNumber, pagination));
            Assert.NotNull(found);
            Assert.AreEqual(expectedCount, found.Count);
            Assert.AreEqual(users.Count, pagination.TotalQuantityOfItems);

            if (expectedCount > 0)
            {
                var indexFirst = (currentPage - 1) * itemsPerPage;
                var firstItem = users[indexFirst];
                var lastItem = users[indexFirst + expectedCount - 1];

                Assert.AreEqual(firstItem.Password, found.First().Password);
                Assert.AreEqual(lastItem.Password, found.Last().Password);
            }

            //Remove
            RemoveRangeUser(usersArray);
        }
    }
}

