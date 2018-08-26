#if NET461
using System;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Exception;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Model;
using Fluent.Architecture.Service;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Specifications;
using Fluent.Architecture.Test.Mock;
using Xunit;

namespace Fluent.Architecture.Test.InternalTests
{
    public partial class BaseServiceTest
    {

        #region SETUP

        public TransactionalService Service { get; set; }
        public BaseController Controller { get; set; }

        public BaseServiceTest()
        {
            Setup.Initialize();

            Controller = MockUtil.GetMockController(typeof(UserController));
            Service = ServiceFactory.Create<UserService>(MockUtil.GetHttpContext());
        }

        #endregion

        [Theory]
        [InlineData(nameof(UserController.Count), 1, true)]
        [InlineData(nameof(UserController.Count), 1, false)]
        [InlineData(nameof(UserController.Count), 0, true)]
        [InlineData(nameof(UserController.Count), 0, false)]
        public void CountSuccessTest(string method, int expectedCount, bool userSelectSpec)
        {
            var user = UserTestUtil.GetNew();
            var passwordForFind = expectedCount == 0 ? user.Password + "xpto" : user.Password;
            var spec = userSelectSpec ? new UserByPassword(Service, passwordForFind) as BaseSpecification : new UserIdByPassword(Service, passwordForFind);

            //Add
            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Add), user);

            var count = TestUtil.Execute<int>(typeof(UserController), method, spec);
            Assert.Equal(expectedCount, count);

            //Remove
            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Remove), user);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void ExistsSuccessTest(bool expectedExists, bool userSelectSpec)
        {
            var user = UserTestUtil.GetNew();
            var passwordForFind = expectedExists ? user.Password : user.Password + "xpto";
            var spec = userSelectSpec ? new UserByPassword(Service, passwordForFind) as BaseSpecification : new UserIdByPassword(Service, passwordForFind);

            //Add
            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Add), user);

            var exists = TestUtil.Execute<bool>(typeof(UserController), nameof(UserController.Exists), spec);
            Assert.Equal(expectedExists, exists);

            //Remove
            TestUtil.Execute<User>(typeof(UserController), nameof(UserController.Remove), user);
        }

        [Fact]
        public void BaseControllerTest()
        {
            var id = Service.SessionRequestId;

            Assert.NotEqual(id, Guid.Empty);
        }

        [Fact]
        public void InitializeServiceFail()
        {
            var service = new LocalTestService();
            var ex = Assert.Throws<IncorrectDevelopmentException>(() => service.SetUserSessionForTest(new UserSessionRequest()));
            Assert.Equal("You can not initialize the FluentService", ex.Message);
        }
    }
}
#endif