#if NET461

using System;
using System.Linq;
using System.Runtime.InteropServices;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Exceptions.ValidationException;
using Fluent.Architecture.Sample.Test.SupportElements;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Sample.Test.TestTools;
using Fluent.Architecture.Test;
using Fluent.Architecture.Test.Mock;
using Fluent.Architecture.Validation;
using NUnit.Framework;

namespace Fluent.Architecture.Sample.Test.Test
{
    [TestFixture]
    [ComVisible(true)]
    public class FluentValidationsTest : FluentInternalTest
    {
        [Theory]
        [TestCase(nameof(FluentFullController<User>.Add))]
        [TestCase(nameof(FluentFullController<User>.Update))]
        public void NullParameterTestFail(string method)
        {
            var error = TestUtil.Execute<ContextFluentValidationException>(this.UserControllerInstance, method, new object[] { null });

            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<NullParameterFluentValidationException>(error.Inconsistencies.First());
        }

        [Theory]
        [TestCase(nameof(FluentFullController<User>.Add))]
        [TestCase(nameof(FluentFullController<User>.Update))]
        public void NullAllKeyNullTestFail(string method)
        {
            var user = InternalTestUtil.GetNewUser();
            user.PersonType = null;
            var error = TestUtil.Execute<ContextFluentValidationException>(this.UserControllerInstance, method, user);

            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<PropertyRequiredFluentValidationException>(error.Inconsistencies.First());
        }

        [Theory]
        [TestCase(nameof(FluentFullController<User>.Add))]
        public void NullOneKeyNullTestAddFail(string method)
        {
            var student = InternalTestUtil.GetNewStudent();
            student.Id = TestUtil.NextRandom();
            var error = TestUtil.Execute<ContextFluentValidationException>(this.StudentControllerInstance, method, student);

            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<FluentPropertyValidationException>(error.Inconsistencies.First());
            Assert.AreEqual("The key must not be entered for this operation.", error.Inconsistencies.First().Message);
        }

        [Theory]
        [TestCase(nameof(FluentFullController<User>.Update))]
        public void NullOneKeyNullTestUpdateFail(string method)
        {
            var student = InternalTestUtil.GetNewStudent();
            student.Id = 0;
            var error = TestUtil.Execute<ContextFluentValidationException>(this.StudentControllerInstance, method, student);

            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<PropertyRequiredFluentValidationException>(error.Inconsistencies.First());
            Assert.AreEqual($"The property {nameof(Student.Id)} must have a value for this operation.", error.Inconsistencies.First().Message);
        }

        [Test]
        public void EntityExistsInDatabaseAddFail()
        {
            var user = InternalTestUtil.GetNewUser();
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Add), user);
            Assert.NotNull(user);

            var error = TestUtil.Execute<ContextFluentValidationException>(this.UserControllerInstance, nameof(FluentFullController<User>.Add), user);

            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<EntityExistsFluentValidationException>(error.Inconsistencies.First());
        }

        // [Test]
        // public void EntityExistsInDatabaseUpdateFail()
        // {
        // var user1 = InternalTestUtil.GetNewUser();
        // user1 = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Add), user1);
        // Assert.NotNull(user1);

        // var user2 = InternalTestUtil.GetNewUser();
        // user2 = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Add), user2);
        // Assert.NotNull(user2);

        // var error = TestUtil.Execute<ContextFluentValidationException>(UserControllerInstance, nameof(UserController.Update), user1);

        // Assert.NotNull(error);
        // Assert.Single(error.Inconsistencies);
        // Assert.IsAssignableFrom<EntityExistsFluentValidationException>(error.Inconsistencies.First());
        // }
        [Theory]
        [TestCase(nameof(FluentFullController<User>.Update), "")]
        [TestCase(nameof(FluentFullController<User>.Add), "")]
        [TestCase(nameof(FluentFullController<User>.Update), " ")]
        [TestCase(nameof(FluentFullController<User>.Add), " ")]
        [TestCase(nameof(FluentFullController<User>.Update), null)]
        [TestCase(nameof(FluentFullController<User>.Add), null)]
        public void RequiredAddAndUpdateTestFail(string method, string name)
        {
            var user = InternalTestUtil.GetNewUser();

            if (method == nameof(FluentFullController<User>.Update))
            {
                user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Add), user);
            }

            user.Name = name;

            var error = TestUtil.Execute<ContextFluentValidationException>(this.UserControllerInstance, method, user);

            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<PropertyRequiredFluentValidationException>(error.Inconsistencies.First());

            if (method == nameof(FluentFullController<User>.Update))
            {
                TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Remove), user);
            }
        }

        [Theory]
        [TestCase(nameof(FluentFullController<User>.Update))]
        [TestCase(nameof(FluentFullController<User>.Remove))]
        public void UpdateAndRemoveNotFoundFail(string method)
        {
            var user = new User
            {
                Name = $"test{Guid.NewGuid()}@mail.com",
                PersonType = ePersonType.User,
                Id = new Random().Next(1, int.MaxValue)
            };

            var err = TestUtil.Execute<ContextFluentValidationException>(this.UserControllerInstance, method, user);
            Assert.NotNull(err);
            Assert.AreEqual(1, err.Inconsistencies.Count);
            Assert.IsAssignableFrom<EntityNotFoundFluentValidationException>(err.Inconsistencies.First());
            Assert.True(err.ValidationError);
        }

        [Theory]
        [TestCase(nameof(FluentFullController<User>.Update))]
        [TestCase(nameof(FluentFullController<User>.Remove))]
        public void UpdateAndRemoveNotKeyValueFail(string method)
        {
            var user = new User
            {
                Name = $"test{Guid.NewGuid()}@mail.com",
                PersonType = ePersonType.User,
            };

            var objectReturn = TestUtil.Execute<ContextFluentValidationException>(this.UserControllerInstance, method, user);
            Assert.NotNull(objectReturn);
            Assert.AreEqual(1, objectReturn.Inconsistencies.Count);
            Assert.IsAssignableFrom<PropertyRequiredFluentValidationException>(objectReturn.Inconsistencies.First());

            objectReturn.Inconsistencies.Clear();
        }

        [Test]
        public void AddUpdateAndRemoveSuccess()
        {
            var user = InternalTestUtil.GetNewUser();

            // Add
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Add), user);
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Find), user);
            Assert.NotNull(user);

            // Update
            user.Name = "New name";
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Update), user);
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Find), user);
            Assert.NotNull(user);
            Assert.AreEqual("New name", user.Name);

            // Remove
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Remove), user);
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Find), user);
            Assert.Null(user);
        }

        [Test]
        public void FullNameAddValidationFail()
        {
            var user = InternalTestUtil.GetNewUser();
            user.Name = "Maria";

            var error = TestUtil.Execute<ContextFluentValidationException>(this.UserControllerInstance, nameof(FluentFullController<User>.Add), user);
            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<FluentPropertyValidationException>(error.Inconsistencies.First());
            Assert.AreEqual(nameof(User.Name), ((FluentPropertyValidationException)error.Inconsistencies.First()).Property);
        }

        [Test]
        public void FullNameUpdateValidationFail()
        {
            var user = InternalTestUtil.GetNewUser();
            user.Name = "Maria Santos";

            TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Add), user);
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Find), user);
            Assert.NotNull(user);

            // Update
            user.Name = "Maria";
            var error = TestUtil.Execute<ContextFluentValidationException>(this.UserControllerInstance, nameof(FluentFullController<User>.Update), user);
            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<FluentPropertyValidationException>(error.Inconsistencies.First());
            Assert.AreEqual(nameof(User.Name), ((FluentPropertyValidationException)error.Inconsistencies.First()).Property);

            // Remove
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Remove), user);
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Find), user);
            Assert.Null(user);
        }

        [Test]
        public void FullNameAddAndUpdateValidationSuccess()
        {
            var user = InternalTestUtil.GetNewUser();
            user.Name = "Maria Santos";

            // Add
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Add), user);
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Find), user);
            Assert.NotNull(user);

            // Update
            user.Name = "New name";
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Update), user);
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Find), user);
            Assert.NotNull(user);
            Assert.AreEqual("New name", user.Name);

            // Remove
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Remove), user);
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(FluentFullController<User>.Find), user);
            Assert.Null(user);
        }

    }
}
#endif
