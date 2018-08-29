#if NET461

using System;
using System.Linq;

using Fluent.Architecture.Test.SupportElements;
using Fluent.Architecture.Test.SupportElements.Controllers;
using Fluent.Architecture.Test.SupportElements.Mock;
using Fluent.Architecture.Test.SupportElements.Model;
using Fluent.Architecture.Test.TestTools;
using Fluent.Architecture.Validation;
using NUnit.Framework;

namespace Fluent.Architecture.Test.Test
{
    using Fluent.Architecture.Exceptions.ValidationException;

    [TestFixture]
    public class FluentValidationsTest : FluentInternalTest
    {
        [Theory]
        [TestCase(nameof(UserController.Add))]
        [TestCase(nameof(UserController.Update))]
        public void NullParameterTestFail(string method)
        {
            var error = TestUtil.Execute<ContextFluentValidation>(UserControllerInstance, method, new object[] { null });

            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<NullParameterFluentValidationException>(error.Inconsistencies.First());
        }

        [Theory]
        [TestCase(nameof(UserController.Add))]
        [TestCase(nameof(UserController.Update))]
        public void NullAllKeyNullTestFail(string method)
        {
            var user = InternalTestUtil.GetNewUser();
            user.PersonType = null;
            var error = TestUtil.Execute<ContextFluentValidation>(UserControllerInstance, method, user);

            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<PropertyRequiredFluentValidationException>(error.Inconsistencies.First());
        }

        [Test]
        public void NullOneKeyNullTestFail()
        {
            var student = InternalTestUtil.GetNewStudent();
            student.Id = TestUtil.NextRandom();
            var error = TestUtil.Execute<ContextFluentValidation>(StudentControllerInstance, nameof(UserController.Add), student);

            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<FluentPropertyValidationException>(error.Inconsistencies.First());
            Assert.AreEqual("The key must not be entered for this operation.", error.Inconsistencies.First().Message);
        }

        [Test]
        public void EntityExistsInDatabaseAddFail()
        {
            var user = InternalTestUtil.GetNewUser();
            user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Add), user);
            Assert.NotNull(user);

            var error = TestUtil.Execute<ContextFluentValidation>(UserControllerInstance, nameof(UserController.Add), user);

            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<EntityExistsFluentValidationException>(error.Inconsistencies.First());
        }

        //[Test]
        //public void EntityExistsInDatabaseUpdateFail()
        //{
        //    var user1 = InternalTestUtil.GetNewUser();
        //    user1 = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Add), user1);
        //    Assert.NotNull(user1);

        //    var user2 = InternalTestUtil.GetNewUser();
        //    user2 = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Add), user2);
        //    Assert.NotNull(user2);

        //    var error = TestUtil.Execute<ContextFluentValidation>(UserControllerInstance, nameof(UserController.Update), user1);

        //    Assert.NotNull(error);
        //    Assert.Single(error.Inconsistencies);
        //    Assert.IsAssignableFrom<EntityExistsFluentValidationException>(error.Inconsistencies.First());
        //}

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
                user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Add), user);
            }

            user.Name = name;

            var error = TestUtil.Execute<ContextFluentValidation>(UserControllerInstance, method, user);

            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<PropertyRequiredFluentValidationException>(error.Inconsistencies.First());

            if (method == nameof(UserController.Update))
            {
                TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Remove), user);
            }
        }

        [Theory]
        [TestCase(nameof(UserController.Update))]
        [TestCase(nameof(UserController.Remove))]
        public void UpdateAndRemoveNotFoundFail(string method)
        {
            var user = new User
            {
                Name = $"test{Guid.NewGuid()}@mail.com",
                PersonType = ePersonType.User,
                Id = new Random().Next(1, int.MaxValue)
            };

            var err = TestUtil.Execute<ContextFluentValidation>(UserControllerInstance, method, user);
            Assert.NotNull(err);
            Assert.AreEqual(1, err.Inconsistencies.Count);
            Assert.IsAssignableFrom<EntityNotFoundFluentValidationException>(err.Inconsistencies.First());
            Assert.True(err.ValidationError);
        }

        [Theory]
        [TestCase(nameof(UserController.Update))]
        [TestCase(nameof(UserController.Remove))]
        public void UpdateAndRemoveNotKeyValueFail(string method)
        {
            var user = new User
            {
                Name = $"test{Guid.NewGuid()}@mail.com",
                PersonType = ePersonType.User,
            };

            var objectReturn = TestUtil.Execute<ContextFluentValidation>(UserControllerInstance, method, user);
            Assert.NotNull(objectReturn);
            Assert.AreEqual(1, objectReturn.Inconsistencies.Count);
            Assert.IsAssignableFrom<PropertyRequiredFluentValidationException>(objectReturn.Inconsistencies.First());

            objectReturn.Inconsistencies.Clear();
        }

        [Test]
        public void AddUpdateAndRemoveSuccess()
        {
            var user = InternalTestUtil.GetNewUser();

            //Add
            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Add), user);
            user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Find), user);
            Assert.NotNull(user);

            //Update
            user.Name = "New name";
            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Update), user);
            user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Find), user);
            Assert.NotNull(user);
            Assert.AreEqual("New name", user.Name);

            //Remove
            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Remove), user);
            user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Find), user);
            Assert.Null(user);
        }

        [Test]
        public void FullNameAddValidationFail()
        {
            var user = InternalTestUtil.GetNewUser();
            user.Name = "Maria";

            var error = TestUtil.Execute<ContextFluentValidation>(UserControllerInstance, nameof(UserController.Add), user);
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

            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Add), user);
            user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Find), user);
            Assert.NotNull(user);

            //Update
            user.Name = "Maria";
            var error = TestUtil.Execute<ContextFluentValidation>(UserControllerInstance, nameof(UserController.Update), user);
            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<FluentPropertyValidationException>(error.Inconsistencies.First());
            Assert.AreEqual(nameof(User.Name), ((FluentPropertyValidationException)error.Inconsistencies.First()).Property);

            //Remove
            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Remove), user);
            user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Find), user);
            Assert.Null(user);
        }

        [Test]
        public void FullNameAddAndUpdateValidationSuccess()
        {
            var user = InternalTestUtil.GetNewUser();
            user.Name = "Maria Santos";

            //Add
            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Add), user);
            user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Find), user);
            Assert.NotNull(user);

            //Update
            user.Name = "New name";
            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Update), user);
            user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Find), user);
            Assert.NotNull(user);
            Assert.AreEqual("New name", user.Name);

            //Remove
            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Remove), user);
            user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Find), user);
            Assert.Null(user);
        }

    }
}
#endif
