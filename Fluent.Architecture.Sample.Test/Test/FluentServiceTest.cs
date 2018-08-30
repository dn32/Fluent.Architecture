#if NET461
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Model;
using Fluent.Architecture.Sample.Test.SupportElements;
using Fluent.Architecture.Sample.Test.SupportElements.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Sample.Test.SupportElements.Specifications;
using Fluent.Architecture.Sample.Test.TestTools;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test;
using NUnit.Framework;

namespace Fluent.Architecture.Sample.Test.Test
{
    [TestFixture]
    [ComVisible(true)]
    public class FluentServiceTest : FluentInternalTest
    {
        [Theory]
        [TestCase(nameof(UserController.Count), 1, true)]
        [TestCase(nameof(UserController.Count), 1, false)]
        [TestCase(nameof(UserController.Count), 0, true)]
        [TestCase(nameof(UserController.Count), 0, false)]
        public void CountSuccessTest(string method, int expectedCount, bool userSelectSpec)
        {
            var user = InternalTestUtil.GetNewUser();
            var passwordForFind = expectedCount == 0 ? user.Password + "xpto" : user.Password;
            var spec = userSelectSpec ? new UserByPassword(this.UserControllerInstance, passwordForFind) as BaseSpecification<User> : new UserIdByPassword(this.UserControllerInstance, passwordForFind);

            //Add
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Add), user);

            var count = TestUtil.Execute<int>(this.UserControllerInstance, method, spec);
            Assert.AreEqual(expectedCount, count);

            //Remove
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Remove), user);
        }

        [Theory]
        [TestCase(true)]
        [TestCase(false)]
        public void FindOneTest(bool success)
        {
            var user = InternalTestUtil.GetNewUser();
            var spec = new UserByEmail(this.UserControllerInstance, success ? user.Email : user.Email + "xxy");

            //Add
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Add), user);
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Find), user);
            var userFound = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.SpecOne), spec);

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
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Remove), user);
        }

        [Test]
        public void FindSelectSpecTest()
        {
            var user1 = InternalTestUtil.GetNewUser();
            var user2 = InternalTestUtil.GetNewUser();
            var password = $"{TestUtil.NextRandom()}{TestUtil.NextRandom()}{TestUtil.NextRandom()}";
            user1.Password = password;
            user2.Password = password;
            var spec = new UserIdByPassword(this.UserControllerInstance, password);

            //Add
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Add), user1);
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Add), user2);

            //List
            var userIds = TestUtil.Execute<List<int>>(this.UserControllerInstance, nameof(UserController.Spec), spec);

            Assert.NotNull(userIds);
            Assert.IsNotEmpty(userIds);
            Assert.AreEqual(2, userIds.Count);

            //Remove
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Remove), user1);
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Remove), user2);
        }

        [Test]
        public void RemoveRangeByEntitiesTest()
        {
            var users = new User[] { InternalTestUtil.GetNewUser(), InternalTestUtil.GetNewUser() };
            var param = new object[] { users };

            //Add
            TestUtil.Execute<User[]>(this.UserControllerInstance, nameof(FluentFullController<User>.AddRange), param);

            //Remove
            TestUtil.Execute<User[]>(this.UserControllerInstance, nameof(UserController.RemoveRange), param);

            var user1 = TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Find), users.First());
            var user2 = TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Find), users.Last());

            Assert.Null(user1);
            Assert.Null(user2);
        }


        [Test]
        public void SessionRequestIdTest()
        {
            Guid PropagateMethodTestB(BaseController controller)
            {
                return ((UserController)controller).SessionRequestId;
            }

            var sessionId = TestUtil.Execute(this.UserControllerInstance, null, null, PropagateMethodTestB);
            Assert.AreNotEqual(Guid.Empty, sessionId);
        }

        [Test]
        public void UserTest()
        {
            object PropagateMethodTestB(BaseController controller)
            {
                return ((UserController)controller).ServiceUser;
            }

            var serviceUser = TestUtil.Execute(this.UserControllerInstance, null, null, PropagateMethodTestB);
            Assert.NotNull(serviceUser);
        }

        [Test]
        public void HttpContextTest()
        {
            HttpContextBase PropagateMethodTestB(BaseController controller)
            {
                return controller.HttpContext;
            }

            var httpContextBase = TestUtil.Execute(this.UserControllerInstance, null, null, PropagateMethodTestB);
            Assert.NotNull(httpContextBase);
        }

        [Test]
        public void ServiceHttpContextTest()
        {
            object PropagateMethodTestB(BaseController controller)
            {
                return ((UserController)controller).ServiceHttpContext;
            }

            var serviceUser = TestUtil.Execute(this.UserControllerInstance, null, null, PropagateMethodTestB);
            Assert.NotNull(serviceUser);
        }

        [Test]
        public void RemoveRangeBySpecTest()
        {
            var users = new User[] { InternalTestUtil.GetNewUser(), InternalTestUtil.GetNewUser() };
            var param = new object[] { users };
            var password = $"{TestUtil.NextRandom()}{TestUtil.NextRandom()}{TestUtil.NextRandom()}";
            users[0].Password = password;
            users[1].Password = password;
            var spec = new UserByPassword(this.UserControllerInstance, password);

            //Add
            TestUtil.Execute<User[]>(this.UserControllerInstance, nameof(FluentFullController<User>.AddRange), param);

            //Remove
            TestUtil.Execute<User[]>(this.UserControllerInstance, nameof(UserController.RemoveRange), spec);

            var user1 = TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Find), users.First());
            var user2 = TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Find), users.Last());

            Assert.Null(user1);
            Assert.Null(user2);
        }

        [Theory]
        [TestCase(true)]
        [TestCase(false)]
        public void FindSelectSpecOneTest(bool success)
        {
            var user = InternalTestUtil.GetNewUser();
            var spec = new UserIdByEmail(this.UserControllerInstance, success ? user.Email : user.Email + "xxy");

            //Add
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Add), user);
            var userId = TestUtil.Execute<int>(this.UserControllerInstance, nameof(UserController.SpecOne), spec);

            if (success)
            {
                Assert.AreNotEqual(0, userId);
            }
            else
            {
                Assert.AreEqual(0, userId);
            }

            //Remove
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Remove), user);
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
            var spec = new UserTelContainsNumber(this.UserControllerInstance, number);

            //Add
            TestUtil.Execute<User[]>(this.UserControllerInstance, nameof(FluentFullController<User>.AddRange), usersParam);

            //Test Count
            var countFound = TestUtil.Execute<int>(this.UserControllerInstance, nameof(UserController.Count), spec);
            Assert.AreEqual(count, countFound);

            //Test SpecSelect
            var usersReturn = TestUtil.Execute<List<User>>(this.UserControllerInstance, nameof(UserController.Spec), spec);
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
            var spec = new UserTelContainsNumber(this.UserControllerInstance, telNumber);

            //Add
            TestUtil.Execute<User[]>(this.UserControllerInstance, nameof(FluentFullController<User>.AddRange), usersParam);

            //Test Count
            var countFound = TestUtil.Execute<int>(this.UserControllerInstance, nameof(UserController.Count), spec);
            Assert.AreEqual(users.Count, countFound);

            var pagination = new FluentPagination(currentPage, itemsPerPage);

            //SpecSelect
            var fount = TestUtil.Execute<List<User>>(this.UserControllerInstance, nameof(UserController.Spec), new object[] { spec, pagination });
            Assert.NotNull(fount);
            Assert.AreEqual(expectedCount, fount.Count);
            Assert.AreEqual(users.Count, pagination.TotalQuantityOfItems);

            if (expectedCount > 0)
            {
                var indexFirst = (currentPage - 1) * itemsPerPage;
                var firstItem = users[indexFirst];
                var lastItem = users[indexFirst + expectedCount - 1];

                Assert.AreEqual(firstItem.Password, fount.First().Password);
                Assert.AreEqual(lastItem.Password, fount.Last().Password);
            }

            //Remove
            TestUtil.Execute<User[]>(this.UserControllerInstance, nameof(UserController.RemoveRange), usersParam);
        }
    }
}
#endif
