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
using System.Web;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Model;
using Fluent.Architecture.Sample.Test.SupportElements;
using Fluent.Architecture.Sample.Test.SupportElements.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Sample.Test.SupportElements.Specifications;
using Fluent.Architecture.Sample.Test.TestTools;
using Fluent.Architecture.Test;
using NUnit.Framework;

namespace Fluent.Architecture.Sample.Test.Test
{
    [TestFixture]
    [ComVisible(true)]
    public class FluentServiceTest : FluentInternalTest
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
            var method = userSelectSpec ? nameof(UserController.CountIdByPassword): nameof(UserController.CountByPassword);

            //Add
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Add), user);

            var count = TestUtil.Execute<int>(this.UserControllerInstance, method, passwordForFind);
            Assert.AreEqual(expectedCount, count);

            //Remove
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Remove), user);
        }

        [Theory]
        [TestCase(true)]
        [TestCase(false)]
        public void FindOneTest(bool success)
        {
            var user = InternalTestUtil.GetNewUser();
            var email = success ? user.Email : user.Email + "xxy";

            //Add
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Add), user);
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Find), user);
            var userFound = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.SpecOne), email);

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
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Remove), user);
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
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Add), user1);
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Add), user2);

            //List
            var userIds = TestUtil.Execute<List<int>>(this.UserControllerInstance, nameof(UserController.ListByIdPassword), password);

            Assert.NotNull(userIds);
            Assert.IsNotEmpty(userIds);
            Assert.AreEqual(2, userIds.Count);

            //Remove
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Remove), user1);
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Remove), user2);
        }

        [Test]
        public void RemoveRangeByEntitiesTest()
        {
            var users = new User[] { InternalTestUtil.GetNewUser(), InternalTestUtil.GetNewUser() };
            var param = new object[] { users };

            //Add
            TestUtil.Execute<User[]>(this.UserControllerInstance, nameof(UserController.AddRange), param);

            //Remove
            TestUtil.Execute<User[]>(this.UserControllerInstance, nameof(UserController.RemoveRange), param);

            var user1 = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Find), users.First());
            var user2 = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Find), users.Last());

            Assert.Null(user1);
            Assert.Null(user2);
        }

        [Test]
        public void SessionRequestIdTest()
        {
            Guid Method(BaseController controller)
            {
                var usercontroller = (UserController)controller;
                var id = usercontroller?.GetType().GetProperty("SessionRequestId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(usercontroller);
                if (id != null)
                {
                    return (Guid)id;
                }

                return default(Guid);
            }

            var sessionId = TestUtil.Execute(this.UserControllerInstance, null, null, Method);
            Assert.AreNotEqual(Guid.Empty, sessionId);
        }

        [Test]
        public void UserTest()
        {
            object Method(BaseController controller)
            {
                var usercontroller = (UserController)controller;
                return usercontroller?.GetType().GetProperty("ServiceUser", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(usercontroller) as ClaimsPrincipal;
            }

            var serviceUser = TestUtil.Execute(this.UserControllerInstance, null, null, Method);
            Assert.NotNull(serviceUser);
        }

        [Test]
        public void HttpContextTest()
        {
            HttpContextBase Method(BaseController controller)
            {
                return controller.HttpContext;
            }

            var httpContextBase = TestUtil.Execute(this.UserControllerInstance, null, null, Method);
            Assert.NotNull(httpContextBase);
        }

        [Test]
        public void ServiceHttpContextTest()
        {
            object Method(BaseController controller)
            {
                var usercontroller = (UserController)controller;
                return usercontroller?.GetType().GetProperty("ServiceHttpContext", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(usercontroller) as HttpContextBase;
            }

            var serviceUser = TestUtil.Execute(this.UserControllerInstance, null, null, Method);
            Assert.NotNull(serviceUser);
            Assert.NotNull(UserControllerInstance.User);
        }

        [Test]
        public void CountTest()
        {
            var user1 = InternalTestUtil.GetNewUser();

            //Add
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Add), user1);

            var count = TestUtil.Execute<int>(this.UserControllerInstance, nameof(UserControllerInstance.Count), null);
            FluentAssert.IsNotNullOrEmpty(count);

            //Remove
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Remove), user1);
        }

        [Test]
        public void FirstOrDefaultTest()
        {
            //Add
            var user1 = InternalTestUtil.GetNewUser();
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Add), user1);

            var user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserControllerInstance.FirstOrDefault), null);
            Assert.NotNull(user);

            //Remove
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Remove), user1);
        }

        [Test]
        public void RemoveRangeBySpecTest()
        {
            var users = new User[] { InternalTestUtil.GetNewUser(), InternalTestUtil.GetNewUser() };
            var param = new object[] { users };
            var password = $"{TestUtil.NextRandom()}{TestUtil.NextRandom()}{TestUtil.NextRandom()}";
            users[0].Password = password;
            users[1].Password = password;

            //Add
            TestUtil.Execute<User[]>(this.UserControllerInstance, nameof(UserController.AddRange), param);

            //Remove
            TestUtil.Execute<User[]>(this.UserControllerInstance, nameof(UserController.RemoveRange), password);

            var user1 = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Find), users.First());
            var user2 = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Find), users.Last());

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
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Add), user);
            var userId = TestUtil.Execute<int>(this.UserControllerInstance, nameof(UserController.SpecOneInt), success ? user.Email : user.Email + "xxy");

            if (success)
            {
                Assert.AreNotEqual(0, userId);
            }
            else
            {
                Assert.AreEqual(0, userId);
            }

            //Remove
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Remove), user);
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

            object[] usersParam = { users.ToArray() };

            //Add
            TestUtil.Execute<User[]>(this.UserControllerInstance, nameof(UserController.AddRange), usersParam);

            //Test Count
            var countFound = TestUtil.Execute<int>(this.UserControllerInstance, nameof(UserController.CountByTelNumber), number);
            Assert.AreEqual(count, countFound);

            //Test SpecSelect
            var usersReturn = TestUtil.Execute<List<User>>(this.UserControllerInstance, nameof(UserController.ListByTelNumber), number);
            Assert.NotNull(usersReturn);
            Assert.AreEqual(count, usersReturn.Count);

            //Remove
            TestUtil.Execute<User[]>(this.UserControllerInstance, nameof(UserController.RemoveRange), usersParam);
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


            object[] usersParam = { users.ToArray() };

            //Add
            TestUtil.Execute<User[]>(this.UserControllerInstance, nameof(UserController.AddRange), usersParam);

            //Test Count
            var countFound = TestUtil.Execute<int>(this.UserControllerInstance, nameof(UserController.CountByTelNumber), telNumber);
            Assert.AreEqual(users.Count, countFound);

            var pagination = new FluentPagination(currentPage, false, itemsPerPage);

            //SpecSelect
            var found = TestUtil.Execute<List<User>>(this.UserControllerInstance, nameof(UserController.ListByTelNumber), new object[] { telNumber, pagination });
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
            TestUtil.Execute<User[]>(this.UserControllerInstance, nameof(UserController.RemoveRange), usersParam);
        }
    }
}

