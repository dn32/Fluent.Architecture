#if NET461
using System;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Service;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests;
using Fluent.Architecture.Test.Mock;
using Xunit;

namespace Fluent.Architecture.Test.InternalTests
{
    public class BaseServiceTest
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

        [Fact]
        public void BaseControllerTest()
        {
            var id = Service.SessionRequestId;

            Assert.NotEqual(id, Guid.Empty);
        }
    }
}
#endif