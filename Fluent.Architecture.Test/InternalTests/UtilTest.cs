using System;
using System.Collections.Generic;
using System.Text;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Service;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Models;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Services;
using Fluent.Architecture.Util;
using Fluent.Architecture.Validation;
using Xunit;

namespace Fluent.Architecture.Test.InternalTests
{
    public class UtilTest
    {
        [Theory]
        [InlineData(typeof(UserService), typeof(User))]
        //[InlineData(typeof(UserSpecification), typeof(User))]
        //[InlineData(typeof(UserValidation), typeof(User))]
        //[InlineData(typeof(UserRepository), typeof(User))]
        [InlineData(typeof(FluentService<User>), typeof(User))]
        [InlineData(typeof(FluentValidation<User>), typeof(User))]
        [InlineData(typeof(FluentRepository<User>), typeof(User))]
        [InlineData(typeof(FluentSpecification<User>), typeof(User))]
        public void GetFluentEntityTypeTest(Type currentType, Type fluentType)
        {
            var fluentTypeFound = GlobalUtil.GetFluentEntityType(currentType);
            Assert.Equal(fluentType, fluentTypeFound);
        }
    }
}
