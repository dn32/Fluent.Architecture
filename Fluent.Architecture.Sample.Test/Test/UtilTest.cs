// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Core.Interfaces;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Sample.Test.SupportElements;
using Fluent.Architecture.Sample.Test.SupportElements.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Sample.Test.SupportElements.Services;
using Fluent.Architecture.Sample.Test.SupportElements.Specifications;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test;
using Fluent.Architecture.Test.Mock;
using Fluent.Architecture.Util;
using Fluent.Architecture.Validation;
using NUnit.Framework;

namespace Fluent.Architecture.Sample.Test.Test
{
    [TestFixture]
    [ComVisible(true)]
    internal class UtilTest : FluentInternalTest
    {
        [Theory]
        [TestCase(typeof(UserController), typeof(User))]
        [TestCase(typeof(UserService), typeof(User))]
        [TestCase(typeof(UserByEmailSpec), typeof(User))]
        [TestCase(typeof(UserValidation), typeof(User))]

        [TestCase(typeof(FluentController<User>), typeof(User))]
        [TestCase(typeof(FluentService<User>), typeof(User))]
        [TestCase(typeof(FluentValidation<User>), typeof(User))]
        [TestCase(typeof(IFluentRepository<User>), typeof(User))]
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
            const string expectedJson = "[]";
            Assert.AreEqual(expectedJson, entity);
        }

        [Test]
        public void GetMethodNameByCallerTypeNullTest()
        {
            var entity = ObjectExtension.GetAllDataOfObject(null);
            const string expectedJson = "[]";
            Assert.AreEqual(expectedJson, entity);
        }

        [Test]
        public void GetMethodNameByCallerTypeListTest()
        {
            var entity = new List<TestEntity> { new TestEntity() }.GetAllDataOfObject();
            const string expectedJson = "[{\"Name\":\"<Id>\",\"Value\":0},{\"Name\":\"_internalString\",\"Value\":\"my value\"},{\"Name\":\"<Id>\",\"Value\":0}]";
            Assert.AreEqual(expectedJson, entity);
        }

        [Test]
        public void InitializeServiceFail()
        {
            var service = new LocalTestService();
            var ex = Assert.Throws<IncorrectDevelopmentException>(() => service.SetUserSessionForTest(new UserSessionRequest()));
            Assert.AreEqual("You can not initialize the FluentService", ex.Message);
        }

        [Theory]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(EnumPersonType.None)]
        [TestCase(null)]
        public void IsFluentNullTest(object _object)
        {
            Assert.True(_object.IsFluentNull());
        }

        [Test]
        public void IsFluentNull2Test()
        {
            Assert.True(Guid.Empty.IsFluentNull());
            Assert.True(string.Empty.IsFluentNull());
            Assert.True(string.Empty.IsFluentNull());
            Func<int> func = () => 1;
            Assert.False(func.IsFluentNull());
        }

        [Theory]
        [TestCase("A", "'A'")]
        [TestCase(" ", "' '")]
        [TestCase(EnumPersonType.None, 0)]
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

            var ex = Assert.Throws<ValidationException>(() => FluentAssert.Equal(user1, user3));
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
        public void ListNext()
        {
            var list = new[] { 1, 2, 3 }.ToList();
            Assert.AreEqual(1, list.Next());
            Assert.AreEqual(2, list.Next());
            Assert.AreEqual(3, list.Next());
            Assert.AreEqual(0, list.Next());
        }

        [Test]
        public void GetKeyValueFail()
        {
            Assert.Throws<InvalidOperationException>(() => new object().GetKeyValue());
        }
    }
}

