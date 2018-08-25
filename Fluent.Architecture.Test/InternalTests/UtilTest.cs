using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Service;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Controllers;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Models;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Repository;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Services;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Specifications.UserSpec;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Validations;
using Fluent.Architecture.Util;
using Fluent.Architecture.Validation;
using Xunit;

namespace Fluent.Architecture.Test.InternalTests
{
    public class UtilTest
    {
        [Theory]
        [InlineData(typeof(UserController), typeof(User))]
        [InlineData(typeof(UserService), typeof(User))]
        [InlineData(typeof(UserByEmail), typeof(User))]
        [InlineData(typeof(UserValidation), typeof(User))]
        [InlineData(typeof(UserRepository), typeof(User))]

        [InlineData(typeof(FluentController<User>), typeof(User))]
        [InlineData(typeof(FluentService<User>), typeof(User))]
        [InlineData(typeof(FluentValidation<User>), typeof(User))]
        [InlineData(typeof(FluentRepository<User>), typeof(User))]
        [InlineData(typeof(FluentSpecification<User>), typeof(User))]


        [InlineData(null, null)]
        [InlineData(typeof(object), null)]
        public void GetFluentEntityTypeTest(Type currentType, Type fluentType)
        {
            var fluentTypeFound = GlobalUtil.GetFluentEntityType(currentType);
            Assert.Equal(fluentType, fluentTypeFound);
        }

        [Fact]
        public void GetMethodNameByCallerTypeTest()
        {
            var name = GlobalUtil.GetMethodNameByCallerType(typeof(UtilTest));
            Assert.Equal(nameof(GetMethodNameByCallerTypeTest), name); 
        }
    }
}
