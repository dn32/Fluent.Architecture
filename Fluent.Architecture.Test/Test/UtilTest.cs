#if NET461
using System;
using System.Collections.Generic;
using System.Linq;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Exception;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Service;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.SupportElements;
using Fluent.Architecture.Test.SupportElements.Controllers;
using Fluent.Architecture.Test.SupportElements.Mock;
using Fluent.Architecture.Test.SupportElements.Model;
using Fluent.Architecture.Test.SupportElements.Services;
using Fluent.Architecture.Test.SupportElements.Specifications;
using Fluent.Architecture.Util;
using Fluent.Architecture.Validation;
using Xunit;

using BaseServiceTest = Fluent.Architecture.Test.SupportElements.Services.BaseServiceTest;

namespace Fluent.Architecture.Test.Test
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
            var entity = new TestEntity { Id = 3, Data = new List<int> { 1, 2 } }.GetAllDataOfObject();
            
            const string expectedJson = "[{\"Name\":\"<Id>\",\"Value\":3},{\"Name\":\"m_value\",\"Value\":1},{\"Name\":\"m_value\",\"Value\":2},{\"Name\":\"_internalString\",\"Value\":\"my value\"},{\"Name\":\"<Id>\",\"Value\":0}]";

            Assert.Equal(expectedJson, entity);
        }

        [Fact]
        public void GetMethodNameByCallerTypeIQueryableTest()
        {
            var entity = new List<TestEntity>().AsQueryable().GetAllDataOfObject();
            const string expectedJson = "[]";
            Assert.Equal(expectedJson, entity);
        }

        [Fact]
        public void GetMethodNameByCallerTypeNullTest()
        {
            var entity = ObjectExtension.GetAllDataOfObject(null);
            const string expectedJson = "[]";
            Assert.Equal(expectedJson, entity);
        }

        [Fact]
        public void GetMethodNameByCallerTypeListTest()
        {
            var entity = new List<TestEntity> { new TestEntity() }.GetAllDataOfObject();
            const string expectedJson = "[{\"Name\":\"<Id>\",\"Value\":0},{\"Name\":\"_internalString\",\"Value\":\"my value\"},{\"Name\":\"<Id>\",\"Value\":0}]";
            Assert.Equal(expectedJson, entity);
        }

        [Fact]
        public void InitializeServiceFail()
        {
            var service = new BaseServiceTest.LocalTestService();
            var ex = Assert.Throws<IncorrectDevelopmentException>(() => service.SetUserSessionForTest(new UserSessionRequest()));
            Assert.Equal("You can not initialize the FluentService", ex.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(ePersonType.None)]
        [InlineData(null)]
        public void IsFluentNullTest(object _object)
        {
            Assert.True(_object.IsFluentNull());
        }

        [Fact]
        public void IsFluentNull2Test()
        {
            Assert.True(Guid.Empty.IsFluentNull());
            Assert.True(String.Empty.IsFluentNull());
            Assert.True(string.Empty.IsFluentNull());
            Func<int> func = () => 1;
            Assert.False(func.IsFluentNull());
        }

        [Theory]
        [InlineData("A", "'A'")]
        [InlineData(" ", "' '")]
        [InlineData(ePersonType.None, 0)]
        [InlineData(null, null)]
        public void GetDbValueTest(object _object, object dbValue)
        {
            Assert.Equal(dbValue, _object.GetDbValue());
        }

        [Fact]
        public void GetDbValue2Test()
        {
            Func<int> func = () => 1;
            Assert.Equal(func.GetDbValue(), func);
            var guid = Guid.NewGuid();
            Assert.Equal(guid.GetDbValue(), $"'{guid}'");
        }
    }
}
#endif
