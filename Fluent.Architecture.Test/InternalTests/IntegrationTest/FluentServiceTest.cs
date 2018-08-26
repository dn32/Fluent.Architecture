#if NET461
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Model;
using Fluent.Architecture.Service;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Specifications;
using Fluent.Architecture.Test.Mock;
using Xunit;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest
{
    public class FluentServiceTest
    {
        #region SETUP

        public TransactionalService Service { get; set; }
        public BaseController Controller { get; set; }

        public FluentServiceTest()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Setup.Initialize(connectionString);

            Controller = MockUtil.GetMockController(typeof(UserController));
            Service = ServiceFactory.Create<UserService>(MockUtil.GetHttpContext());
        }

        #endregion

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void FindOneTest(bool success)
        {
            var user = UserTestUtil.GetNew();
            var spec = new UserByEmail(Service, success ? user.Email : user.Email + "xxy");

            //Add
            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Add), user);
            user = TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Find), user);
            var userFound = TestUtil.Execute<User>(typeof(UserController), nameof(UserController.SpecOne), spec);

            if (success)
            {
                Assert.NotNull(userFound);
                Assert.Equal(user.GetAllDataOfObject(), userFound.GetAllDataOfObject());
            }
            else
            {
                Assert.Null(userFound);
            }

            //Remove
            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Remove), user);
        }

        [Fact]
        public void FindSelectSpecTest()
        {
            var user1 = UserTestUtil.GetNew();
            var user2 = UserTestUtil.GetNew();
            var password = $"{TestUtil.NextRandom()}{TestUtil.NextRandom()}{TestUtil.NextRandom()}";
            user1.Password = password;
            user2.Password = password;
            var spec = new UserIdByPassword(Service, password);

            //Add
            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Add), user1);
            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Add), user2);

            //List
            var userIds = TestUtil.Execute<List<int>>(typeof(UserController), nameof(UserController.Spec), spec);

            Assert.NotNull(userIds);
            Assert.NotEmpty(userIds);
            Assert.Equal(2, userIds.Count);

            //Remove
            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Remove), user1);
            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Remove), user2);
        }

        [Fact]
        public void RemoveRangeByEntitiesTest()
        {
            var users = new User[] { UserTestUtil.GetNew(), UserTestUtil.GetNew() };
            var param = new object[] { users };

            //Add
            TestUtil.Execute<User[]>(typeof(UserController), nameof(UserController.AddRange), param);

            //Remove
            TestUtil.Execute<User[]>(typeof(UserController), nameof(UserController.RemoveRange), param);

            var user1 = TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Find), users.First());
            var user2 = TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Find), users.Last());

            Assert.Null(user1);
            Assert.Null(user2);
        }

        [Fact]
        public void RemoveRangeBySpecTest()
        {
            var users = new User[] { UserTestUtil.GetNew(), UserTestUtil.GetNew() };
            var param = new object[] { users };
            var password = $"{TestUtil.NextRandom()}{TestUtil.NextRandom()}{TestUtil.NextRandom()}";
            users[0].Password = password;
            users[1].Password = password;
            var spec = new UserByPassword(Service, password);

            //Add
            TestUtil.Execute<User[]>(typeof(UserController), nameof(UserController.AddRange), param);

            //Remove
            TestUtil.Execute<User[]>(typeof(UserController), nameof(UserController.RemoveRange), spec);

            var user1 = TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Find), users.First());
            var user2 = TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Find), users.Last());

            Assert.Null(user1);
            Assert.Null(user2);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void FindSelectSpecOneTest(bool success)
        {
            var user = UserTestUtil.GetNew();
            var spec = new UserIdByEmail(Service, success ? user.Email : user.Email + "xxy");

            //Add
            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Add), user);
            var userId = TestUtil.Execute<int>(typeof(UserController), nameof(UserController.SpecOne), spec);

            if (success)
            {
                Assert.NotEqual(0, userId);
            }
            else
            {
                Assert.Equal(0, userId);
            }

            //Remove
            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Remove), user);
        }

        [Theory]
        [InlineData("62", 1)]
        [InlineData("63", 2)]
        [InlineData("64", 0)]
        public void CountTest(string number, int count)
        {
            var user1 = UserTestUtil.GetNew();
            user1.Tel = "00000000000000";

            var users = new List<User> { user1 };

            number += TestUtil.NextRandom();

            for (var i = 0; i < count; i++)
            {
                var user = UserTestUtil.GetNew();
                user.Tel = number + TestUtil.NextRandom();
                users.Add(user);
            }

            object[] usersParam = { users.ToArray() };
            var spec = new UserTelContainsNumber(Service, number);

            //Add
            TestUtil.Execute<User[]>(typeof(UserController), nameof(UserController.AddRange), usersParam);

            //Test Count
            var countFound = TestUtil.Execute<int>(typeof(UserController), nameof(UserController.Count), spec);
            Assert.Equal(count, countFound);

            //Test Spec
            var usersReturn = TestUtil.Execute<List<User>>(typeof(UserController), nameof(UserController.Spec), spec);
            Assert.NotNull(usersReturn);
            Assert.Equal(count, usersReturn.Count);

            //Remove
            TestUtil.Execute<User[]>(typeof(UserController), nameof(UserController.RemoveRange), usersParam);
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(0, 10)]
        [InlineData(5, 7)]
        [InlineData(11, 10)]
        [InlineData(15, 10)]
        public void PaginationTest(int currentPage, int itemsPerPage)
        {
            var users = new List<User>();
            var telNumber = $"{TestUtil.NextRandom()}{TestUtil.NextRandom()}{TestUtil.NextRandom()}";
            for (var i = 1; i <= 103; i++)
            {
                var user = UserTestUtil.GetNew();
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
            var spec = new UserTelContainsNumber(Service, telNumber);

            //Add
            TestUtil.Execute<User[]>(typeof(UserController), nameof(UserController.AddRange), usersParam);

            //Test Count
            var countFound = TestUtil.Execute<int>(typeof(UserController), nameof(UserController.Count), spec);
            Assert.Equal(users.Count, countFound);

            var pagination = new FluentPagination(currentPage, itemsPerPage);

            //Spec
            var fount = TestUtil.Execute<List<User>>(typeof(UserController), nameof(UserController.Spec), spec, pagination);
            Assert.NotNull(fount);
            Assert.Equal(expectedCount, fount.Count);
            Assert.Equal(users.Count, pagination.TotalQuantityOfItems);

            if (expectedCount > 0)
            {
                var indexFirst = (currentPage - 1) * itemsPerPage;
                var firstItem = users[indexFirst];
                var lastItem = users[indexFirst + expectedCount - 1];

                Assert.Equal(firstItem.Password, fount.First().Password);
                Assert.Equal(lastItem.Password, fount.Last().Password);
            }

            //Remove
            TestUtil.Execute<User[]>(typeof(UserController), nameof(UserController.RemoveRange), usersParam);
        }
    }
}
#endif
