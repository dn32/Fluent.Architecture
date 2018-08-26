#if NET461
using System;
using System.Collections.Generic;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Service;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Specifications;
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
            var fluentTypeFound = currentType.GetFluentEntityType();
            Assert.Equal(fluentType, fluentTypeFound);
        }

        [Fact]
        public void GetMethodNameByCallerTypeTest()
        {
            var entity = new BaseServiceTest.TestEntity {Id = 3, Data = new List<int> {1, 2}}.GetAllDataOfObject();

            const string expectedJson = "[{\"Name\":\"<Id>k__BackingField\",\"Value\":3},{\"Name\":\"m_value\",\"Value\":1},{\"Name\":\"m_value\",\"Value\":2},{\"Name\":\"_internalString\",\"Value\":\"InternalStringValue\"}]";

            Assert.Equal(expectedJson, entity);
        }

        //[Fact]
        //public void GetMethodNameByCallerTypeTest()
        //{
        //    var name = GlobalUtil.GetMethodNameByCallerType(typeof(UtilTest));
        //    Assert.Equal(nameof(GetMethodNameByCallerTypeTest), name); 
        //}
    }
}
#endif
