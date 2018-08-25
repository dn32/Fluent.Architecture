#if NET461
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Service;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Controllers;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Services;
using Fluent.Architecture.Test.Mock;
using Fluent.Architecture.Test.Mock.ControllerMock;
using Xunit;

namespace Fluent.Architecture.Test.InternalTests
{
    public class BaseServiceTest
    {

#region SETUP

        public TransactionalService Service { get; set; }
        public BaseController Controller { get; set; }
        public HttpContextBaseMock HttpContext { get; set; }

        public BaseServiceTest()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Setup.Initialize(connectionString);
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