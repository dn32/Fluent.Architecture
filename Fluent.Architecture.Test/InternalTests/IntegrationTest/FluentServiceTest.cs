#if NET461
using System;
using System.Collections.Generic;
using System.Configuration;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Service;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Controllers;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Models;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Services;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Specifications.UserSpec;
using Fluent.Architecture.Test.Mock;
using Fluent.Architecture.Test.Mock.ControllerMock;
using Xunit;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest
{
    public class FluentServiceTest
    {
        #region SETUP

        public TransactionalService Service { get; set; }
        public BaseController Controller { get; set; }
        public HttpContextBaseMock HttpContext { get; set; }

        public FluentServiceTest()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Setup.Initialize(connectionString);
            Controller = MockUtil.GetMockController(typeof(UserController));
            HttpContext = MockUtil.GetHttpContext(Controller.GetType());
            Service = ServiceFactory.Create<UserService>(HttpContext);
        }

        #endregion

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void FindOneTest(bool success)
        {
            var user = UserTestUtil.GetNewUser();
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

        [Theory]
        [InlineData("62", 1)]
        [InlineData("63", 2)]
        [InlineData("64", 0)]
        public void CountTest(string number, int count)
        {
            var user1 = UserTestUtil.GetNewUser();
            var user2 = UserTestUtil.GetNewUser();
            var user3 = UserTestUtil.GetNewUser();

            user1.Tel = "6212345678";
            user2.Tel = "6358454589";
            user3.Tel = "6397166858";
            
            var users = new[] { new[] { user1, user2, user3 } };
            var spec = new UserTelContainsNumber(Service, number);
            var specAll = new UserAll(Service);

            //Clear all user
            TestUtil.Execute<User[]>(typeof(UserController), nameof(UserController.RemoveRange), specAll);

            //Add
            TestUtil.Execute<User[]>(typeof(UserController), nameof(UserController.AddRange), users);

            //Test Count
            var countFound = TestUtil.Execute<int>(typeof(UserController), nameof(UserController.Count), spec);
            Assert.Equal(count, countFound);
            
            //Test Spec
            var usersReturn = TestUtil.Execute<List<User>>(typeof(UserController), nameof(UserController.Spec), spec);
            Assert.NotNull(usersReturn);
            Assert.Equal(count, usersReturn.Count);
            
            //Remove
            TestUtil.Execute<User[]>(typeof(UserController), nameof(UserController.RemoveRange), users);
        }
    }
}
#endif
