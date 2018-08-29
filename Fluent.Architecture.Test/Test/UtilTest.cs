#if NET461
using System;
using System.Collections.Generic;
using System.Linq;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.SupportElements;
using Fluent.Architecture.Test.SupportElements.Controllers;
using Fluent.Architecture.Test.SupportElements.Mock;
using Fluent.Architecture.Test.SupportElements.Model;
using Fluent.Architecture.Test.SupportElements.Services;
using Fluent.Architecture.Test.SupportElements.Specifications;
using Fluent.Architecture.Util;
using Fluent.Architecture.Validation;
using NUnit.Framework;

using BaseServiceTest = Fluent.Architecture.Test.SupportElements.Services.BaseServiceTest;

namespace Fluent.Architecture.Test.Test
{
    using Fluent.Architecture.Exceptions;

    [TestFixture]
    public class UtilTest : FluentInternalTest
    {
        [Theory]
        [TestCase(typeof(UserController), typeof(User))]
        [TestCase(typeof(UserService), typeof(User))]
        [TestCase(typeof(UserByEmail), typeof(User))]
        [TestCase(typeof(UserValidation), typeof(User))]
        [TestCase(typeof(UserRepository), typeof(User))]

        [TestCase(typeof(FluentController<User>), typeof(User))]
        [TestCase(typeof(FluentService<User>), typeof(User))]
        [TestCase(typeof(FluentValidation<User>), typeof(User))]
        [TestCase(typeof(FluentRepository<User>), typeof(User))]
        [TestCase(typeof(FluentSpecification<User>), typeof(User))]

        [TestCase(null, null)]
        [TestCase(typeof(object), null)]
        public void GetFluentEntityTypeTest(Type currentType, Type fluentType)
        {
            var fluentTypeFound = currentType.GetFluentEntityType();
            Assert.AreEqual(fluentType, fluentTypeFound);
        }

        [Test]
        public void GetMethodNameByCallerTypeTest()
        {
            var entity = new TestEntity { Id = 3, Data = new List<int> { 1, 2 } }.GetAllDataOfObject();

            const string expectedJson = "[{\"Name\":\"<Id>\",\"Value\":3},{\"Name\":\"m_value\",\"Value\":1},{\"Name\":\"m_value\",\"Value\":2},{\"Name\":\"_internalString\",\"Value\":\"my value\"},{\"Name\":\"<Id>\",\"Value\":0}]";

            Assert.AreEqual(expectedJson, entity);
        }

        [Test]
        public void GetMethodNameByCallerTypeIQueryableTest()
        {
            var entity = new List<TestEntity>().AsQueryable().GetAllDataOfObject();
            const string ExpectedJson = "[]";
            Assert.AreEqual(ExpectedJson, entity);
        }

        [Test]
        public void GetMethodNameByCallerTypeNullTest()
        {
            var entity = ObjectExtension.GetAllDataOfObject(null);
            const string ExpectedJson = "[]";
            Assert.AreEqual(ExpectedJson, entity);
        }

        [Test]
        public void GetMethodNameByCallerTypeListTest()
        {
            var entity = new List<TestEntity> { new TestEntity() }.GetAllDataOfObject();
            const string ExpectedJson = "[{\"Name\":\"<Id>\",\"Value\":0},{\"Name\":\"_internalString\",\"Value\":\"my value\"},{\"Name\":\"<Id>\",\"Value\":0}]";
            Assert.AreEqual(ExpectedJson, entity);
        }

        [Test]
        public void InitializeServiceFail()
        {
            var service = new BaseServiceTest.LocalTestService();
            var ex = Assert.Throws<IncorrectDevelopmentException>(() => service.SetUserSessionForTest(new UserSessionRequest()));
            Assert.AreEqual("You can not initialize the FluentService", ex.Message);
        }

        [Theory]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(ePersonType.None)]
        [TestCase(null)]
        public void IsFluentNullTest(object _object)
        {
            Assert.True(_object.IsFluentNull());
        }

        [Test]
        public void IsFluentNull2Test()
        {
            Assert.True(Guid.Empty.IsFluentNull());
            Assert.True(String.Empty.IsFluentNull());
            Assert.True(string.Empty.IsFluentNull());
            Func<int> func = () => 1;
            Assert.False(func.IsFluentNull());
        }

        [Theory]
        [TestCase("A", "'A'")]
        [TestCase(" ", "' '")]
        [TestCase(ePersonType.None, 0)]
        [TestCase(null, null)]
        public void GetDbValueTest(object _object, object dbValue)
        {
            Assert.AreEqual(dbValue, _object.GetDbValue());
        }

        [Test]
        public void GetDbValue2Test()
        {
            Func<int> func = () => 1;
            Assert.AreEqual(func.GetDbValue(), func);
            var guid = Guid.NewGuid();
            Assert.AreEqual(guid.GetDbValue(), $"'{guid}'");
        }

        [Test]
        public void FluentAssertTest()
        {
            var user1 = new User { Id = 1 };
            var user2 = new User { Id = 1 };
            var user3 = new User { Id = 3 };

            FluentAssert.Equal(user1, user2);

            var ex = Assert.Throws<System.Exception>(() => FluentAssert.Equal(user1, user3));
            Assert.AreEqual("The objects are different", ex.Message);
        }

        [Test]
        public void FluentTestTest()
        {
            var date = "17/11/85".GetDate();
            Assert.AreEqual(17, date.Day);
            Assert.AreEqual(11, date.Month);
            Assert.AreEqual(1985, date.Year);
        }

        [Test]
        public void GetMethodForPropagationFail()
        {
            var ex = Assert.Throws<IncorrectDevelopmentException>(() => GlobalUtil.GetMethodForPropagation());
            Assert.NotNull(ex);
            Assert.AreEqual("The propagation call could not be traced. Only BaseController child controllers can make propagation call.", ex.Message);
        }
    }
}
#endif
