//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Fluent.Architecture.Exception;
//using Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests.Specifications.UserSpec;
//using Xunit;

//namespace Fluent.Architecture.FrameworkTest.IntegrationTest
//{
//    public class SpecificationTest
//    {
//        #region SETUP

//        public SpecificationTest()
//        {
//            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//            Setup.Initialize(connectionString);
//        }

//        #endregion

//        [Fact]
//        public void IncorrectDevelopmentExceptionTest()
//        {
//            var spec = new UserByEmail();
//        }
//    }
//}
