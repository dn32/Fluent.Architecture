//using System.Configuration;
//using Fluent.Architecture.Factory;
//using Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests;
//using Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests.Services;
//using Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests.Specifications.UserSpec;
//using Xunit;

//namespace Fluent.Architecture.FrameworkTest.IntegrationTest
//{
//    public class FluentServiceTest
//    {
//        #region SETUP

//        public FluentServiceTest()
//        {
//            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//            Setup.Initialize(connectionString);
//        }

//        #endregion

//        [Fact]
//        public void IncorrectDevelopmentExceptionTest()
//        {
//            var userService = ServiceFactory.Create<UserService>()
//            var user = UserTestUtil.GetNewUser();
//            var spec = new UserByEmail(user.Email);
//        }
//    }
//}
