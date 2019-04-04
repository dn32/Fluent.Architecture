// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Runtime.InteropServices;
using Fluent.Architecture.Exceptions.ValidationException;
using Fluent.Architecture.Sample.Test.SupportElements;
using Fluent.Architecture.Sample.Test.SupportElements.Controllers;
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
        [TestCase(nameof(UserController.Add))]
        [TestCase(nameof(UserController.Update))]
        public void NullAllKeyNullTestFail(string method)
        {
            var user = InternalTestUtil.GetNewUser();
            user.PersonType = null;
            var error = TestUtil.Execute<ContextFluentValidationException>(this.UserControllerInstance, method, user);

            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<DbFieldRequiredFluentValidationException>(error.Inconsistencies.First());
        }

        [Theory]
        [TestCase(nameof(UserController.Add))]
        public void NullOneKeyNullTestAddFail(string method)
        {
            var student = InternalTestUtil.GetNewStudent();
            student.Id = TestUtil.NextRandom();
            var error = TestUtil.Execute<ContextFluentValidationException>(this.StudentControllerInstance, method, student);

            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<DbFieldNotRequiredFluentValidationException>(error.Inconsistencies.First());
            Assert.AreEqual("The Id field should not have a value for this operation.", error.Inconsistencies.First().Message);
        }

        [Theory]
        [TestCase(nameof(UserController.Update))]
        public void NullOneKeyNullTestUpdateFail(string method)
        {
            var student = InternalTestUtil.GetNewStudent();
            student.Id = 0;
            var error = TestUtil.Execute<ContextFluentValidationException>(this.StudentControllerInstance, method, student);

            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<DbFieldRequiredFluentValidationException>(error.Inconsistencies.First());
            Assert.AreEqual($"The field {nameof(Student.Id)} must have a value for this operation.", error.Inconsistencies.First().Message);
        }

        [Test]
        public void EntityExistsInDatabaseAddFail()
        {
            var user = InternalTestUtil.GetNewUser();
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Add), user);
            Assert.NotNull(user);

            var error = TestUtil.Execute<ContextFluentValidationException>(this.UserControllerInstance, nameof(UserController.Add), user);

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
        [TestCase(nameof(UserController.Update), "")]
        [TestCase(nameof(UserController.Add), "")]
        [TestCase(nameof(UserController.Update), " ")]
        [TestCase(nameof(UserController.Add), " ")]
        [TestCase(nameof(UserController.Update), null)]
        [TestCase(nameof(UserController.Add), null)]
        public void RequiredAddAndUpdateTestFail(string method, string name)
        {
            var user = InternalTestUtil.GetNewUser();

            if (method == nameof(UserController.Update))
            {
                user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Add), user);
            }

            user.Name = name;

            var error = TestUtil.Execute<ContextFluentValidationException>(this.UserControllerInstance, method, user);

            Assert.NotNull(error);
            Assert.AreEqual(2, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<UiFieldRequiredFluentValidationException>(error.Inconsistencies.First());
            Assert.IsAssignableFrom<UiFieldRequiredFluentValidationException>(error.Inconsistencies.Last());

            if (method == nameof(UserController.Update))
            {
                TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Remove), user);
            }
        }

        [Theory]
        [TestCase(nameof(UserController.Update))]
        public void UpdateAndUpdateNotFoundFail(string method)
        {
            var user = new User
            {
                Name = $"test{Guid.NewGuid()}@mail.com",
                PersonType = ePersonType.User,
                Id = new Random().Next(1, int.MaxValue)
            };

            var error = TestUtil.Execute<ContextFluentValidationException>(this.UserControllerInstance, method, user);
            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<UiFieldRequiredFluentValidationException>(error.Inconsistencies.First());
            Assert.True(error.ValidationError);
        }

        [Theory]
        [TestCase(nameof(UserController.Remove))]
        public void UpdateAndRemoveNotFoundFail(string method)
        {
            var user = new User
            {
                Name = $"test{Guid.NewGuid()}@mail.com",
                PersonType = ePersonType.User,
                Id = new Random().Next(1, int.MaxValue)
            };

            var error = TestUtil.Execute<ContextFluentValidationException>(this.UserControllerInstance, method, user);
            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<EntityNotFoundFluentValidationException>(error.Inconsistencies.Last());
            Assert.True(error.ValidationError);
        }
        
        [Theory]
        [TestCase(nameof(UserController.Update))]
        public void UpdateAndUpdateNotKeyValueFail(string method)
        {
            var user = new User
            {
                Name = $"test{Guid.NewGuid()}@mail.com",
                PersonType = ePersonType.User,
            };

            var objectReturn = TestUtil.Execute<ContextFluentValidationException>(this.UserControllerInstance, method, user);
            Assert.NotNull(objectReturn);
            Assert.AreEqual(2, objectReturn.Inconsistencies.Count);
            Assert.IsAssignableFrom<UiFieldRequiredFluentValidationException>(objectReturn.Inconsistencies.First());
            Assert.IsAssignableFrom<DbFieldRequiredFluentValidationException>(objectReturn.Inconsistencies.Last());

            objectReturn.Inconsistencies.Clear();
        }

        [Theory]
        [TestCase(nameof(UserController.Remove))]
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
            Assert.IsAssignableFrom<DbFieldRequiredFluentValidationException>(objectReturn.Inconsistencies.Last());

            objectReturn.Inconsistencies.Clear();
        }

        [Test]
        public void AddUpdateAndRemoveSuccess()
        {
            var user = InternalTestUtil.GetNewUser();

            // Add
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Add), user);
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Find), user);
            Assert.NotNull(user);

            // Update
            user.Name = "New name";
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Update), user);
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Find), user);
            Assert.NotNull(user);
            Assert.AreEqual("New name", user.Name);

            // Remove
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Remove), user);
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Find), user);
            Assert.Null(user);
        }

        [Test]
        public void FullNameAddValidationFail()
        {
            var user = InternalTestUtil.GetNewUser();
            user.Name = "Maria";

            var error = TestUtil.Execute<ContextFluentValidationException>(this.UserControllerInstance, nameof(UserController.Add), user);
            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<UiFieldRequiredFluentValidationException>(error.Inconsistencies.First());
            var validationError = (FluentPropertyValidationException)error.Inconsistencies.First();
            Assert.AreEqual("Full Name", validationError.Values.First());
        }

        [Test]
        public void FullNameUpdateValidationFail()
        {
            var user = InternalTestUtil.GetNewUser();
            user.Name = "Maria Santos";

            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Add), user);
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Find), user);
            Assert.NotNull(user);

            // Update
            user.Name = "Maria";
            var error = TestUtil.Execute<ContextFluentValidationException>(this.UserControllerInstance, nameof(UserController.Update), user);
            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<UiFieldRequiredFluentValidationException>(error.Inconsistencies.First());
            var validationError = (FluentPropertyValidationException)error.Inconsistencies.First();
            Assert.AreEqual("Full Name", validationError.Values.First());

            // Remove
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Remove), user);
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Find), user);
            Assert.Null(user);
        }

        [Test]
        public void FullNameAddAndUpdateValidationSuccess()
        {
            var user = InternalTestUtil.GetNewUser();
            user.Name = "Maria Santos";

            // Add
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Add), user);
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Find), user);
            Assert.NotNull(user);

            // Update
            user.Name = "New name";
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Update), user);
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Find), user);
            Assert.NotNull(user);
            Assert.AreEqual("New name", user.Name);

            // Remove
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Remove), user);
            user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Find), user);
            Assert.Null(user);
        }

    }
}

