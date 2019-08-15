//// -----------------------------------------------------------------------
//// <copyright company="Fluent System">
////     Copyright © Fluent System. All rights reserved.
////     TODOS OS DIREITOS RESERVADOS.
//// </copyright>
//// -----------------------------------------------------------------------

//using System;
//using System.Linq;
//using System.Runtime.InteropServices;
//using Fluent.Architecture.Exceptions.ValidationException;
//using Fluent.Architecture.Sample.Test.SupportElements;
//using Fluent.Architecture.Sample.Test.SupportElements.Controllers;
//using Fluent.Architecture.Sample.Test.SupportElements.Model;
//using Fluent.Architecture.Sample.Test.TestTools;
//using Fluent.Architecture.Test;
//using Fluent.Architecture.Test.Mock;
//using Fluent.Architecture.Validation;
//using NUnit.Framework;

//namespace Fluent.Architecture.Sample.Test.Test
//{
//    //Todo - Validar todos os range com lista vazia
//    [TestFixture]
//    [ComVisible(true)]
//    internal class FluentValidationsTest : FluentInternalTest
//    {
//        [Test]
//        public void NullAllKeyNullTestFailUpdate()
//        {
//            var user = InternalTestUtil.GetNewUser();
//            user.PersonType = null;
//            var error = Assert.Throws<ContextFluentValidationException>(() =>
//            {
//                UpdateUser(user);
//            });

//            Assert.NotNull(error);
//            Assert.AreEqual(2, error.Inconsistencies.Count);
//            Assert.IsAssignableFrom<UiFieldRequiredFluentValidationException>(error.Inconsistencies.First());
//        }

//        [Test]
//        public void NullAllKeyNullTestFailAdd()
//        {
//            var user = InternalTestUtil.GetNewUser();
//            user.PersonType = null;
//            var error = Assert.Throws<ContextFluentValidationException>(() =>
//            {
//                TestUtil.Execute<UserController, object>(UserControllerInstance, (UserController controller) => controller.Add(user));
//            });

//            Assert.NotNull(error);
//            Assert.AreEqual(1, error.Inconsistencies.Count);
//            Assert.IsAssignableFrom<UiFieldRequiredFluentValidationException>(error.Inconsistencies.First());
//        }

//        [Test]
//        public void NullOneKeyNullTestUpdateFail()
//        {
//            var student = InternalTestUtil.GetNewStudent();
//            student.Id = 0;
//            var error = Assert.Throws<ContextFluentValidationException>(() =>
//            {
//                TestUtil.Execute<StudentController, object>(StudentControllerInstance, (StudentController controller) => controller.Update(student));
//            });

//            Assert.NotNull(error);
//            Assert.AreEqual(1, error.Inconsistencies.Count);
//            Assert.IsAssignableFrom<DbFieldRequiredFluentValidationException>(error.Inconsistencies.First());
//            Assert.AreEqual($"The field {nameof(Student.Id)} must have a value for this operation.", error.Inconsistencies.First().Message);
//        }

//        [Test]
//        public void EntityExistsInDatabaseAddFail()
//        {
//            var user = InternalTestUtil.GetNewUser();

//            AddUser(user);
//            user = FindUser(user);

//            Assert.NotNull(user);

//            var error = Assert.Throws<ContextFluentValidationException>(() =>
//            {
//                AddUser(user);
//            });

//            Assert.NotNull(error);
//            Assert.AreEqual(1, error.Inconsistencies.Count);
//            Assert.IsAssignableFrom<EntityExistsFluentValidationException>(error.Inconsistencies.First());
//        }

//        [Theory]
//        [TestCase(nameof(UserController.Update), "")]
//        [TestCase(nameof(UserController.Add), "")]
//        [TestCase(nameof(UserController.Update), " ")]
//        [TestCase(nameof(UserController.Add), " ")]
//        [TestCase(nameof(UserController.Update), null)]
//        [TestCase(nameof(UserController.Add), null)]
//        public void RequiredAddAndUpdateTestFail(string method, string name)
//        {
//            var user = InternalTestUtil.GetNewUser();

//            if (method == nameof(UserController.Update))
//            {
//                AddUser(user);
//                user = FindUser(user);
//            }

//            user.Name = name;

//            var error = Assert.Throws<ContextFluentValidationException>(() =>
//            {
//                if (method == nameof(UserController.Update))
//                {
//                    UpdateUser(user);
//                }
//                else
//                {
//                    TestUtil.Execute<UserController, object>(UserControllerInstance, (UserController controller) => controller.Add(user));
//                }
//            });

//            Assert.NotNull(error);
//            Assert.AreEqual(2, error.Inconsistencies.Count);
//            Assert.IsAssignableFrom<UiFieldRequiredFluentValidationException>(error.Inconsistencies.First());
//            Assert.IsAssignableFrom<UiFieldRequiredFluentValidationException>(error.Inconsistencies.Last());

//            if (method == nameof(UserController.Update))
//            {
//                RemoveUser(user);
//            }
//        }

//        [Test]
//        public void UpdateNotFoundFail()
//        {
//            var user = new User
//            {
//                Email = $"test{Guid.NewGuid()}@mail.com",
//                UserName = $"test{Guid.NewGuid()}@mail.com",
//                PersonType = EnumPersonType.User,
//                Name = "name name",
//                Id = new Random().Next(1, int.MaxValue)
//            };

//            var error = Assert.Throws<ContextFluentValidationException>(() =>
//            {
//                UpdateUser(user);
//            });

//            Assert.NotNull(error);
//            Assert.AreEqual(1, error.Inconsistencies.Count);
//            Assert.IsAssignableFrom<EntityNotFoundFluentValidationException>(error.Inconsistencies.First());
//            Assert.True(error.ValidationError);
//        }

//        [Test]
//        public void RemoveNotFoundFail()
//        {
//            var user = new User
//            {
//                Name = $"test{Guid.NewGuid()}@mail.com",
//                PersonType = EnumPersonType.User,
//                Id = new Random().Next(1, int.MaxValue)
//            };

//            var error = Assert.Throws<ContextFluentValidationException>(() =>
//            {
//                RemoveUser(user);
//            });

//            Assert.NotNull(error);
//            Assert.AreEqual(1, error.Inconsistencies.Count);
//            Assert.IsAssignableFrom<EntityNotFoundFluentValidationException>(error.Inconsistencies.Last());
//            Assert.True(error.ValidationError);
//        }

//        [Test]
//        public void UpdateAndUpdateNotKeyValueFail()
//        {
//            var user = new User
//            {
//                Name = $"test{Guid.NewGuid()}@mail.com",
//                PersonType = EnumPersonType.User,
//            };

//            var error = Assert.Throws<ContextFluentValidationException>(() =>
//            {
//                UpdateUser(user);
//            });

//            Assert.NotNull(error);
//            Assert.AreEqual(2, error.Inconsistencies.Count);
//            Assert.IsAssignableFrom<UiFieldRequiredFluentValidationException>(error.Inconsistencies.First());
//            Assert.IsAssignableFrom<DbFieldRequiredFluentValidationException>(error.Inconsistencies.Last());

//            error.Inconsistencies.Clear();
//        }

//        [Test]
//        public void UpdateAndRemoveNotKeyValueFail()
//        {
//            var user = new User
//            {
//                Name = $"test{Guid.NewGuid()}@mail.com",
//                PersonType = EnumPersonType.User,
//            };

//            var error = Assert.Throws<ContextFluentValidationException>(() =>
//            {
//                RemoveUser(user);
//            });

//            Assert.NotNull(error);
//            Assert.AreEqual(2, error.Inconsistencies.Count);
//            Assert.IsAssignableFrom<DbFieldRequiredFluentValidationException>(error.Inconsistencies.First());
//            Assert.IsAssignableFrom<EntityNotFoundFluentValidationException>(error.Inconsistencies.Last());

//            error.Inconsistencies.Clear();
//        }

//        [Test]
//        public void AddUpdateAndRemoveSuccess()
//        {
//            var user = InternalTestUtil.GetNewUser();

//            // Add
//            AddUser(user);
//            user = FindUser(user);
//            Assert.NotNull(user);

//            // Update
//            user.Name = "New name";
//            UpdateUser(user);
//            user = FindUser(user);
//            Assert.NotNull(user);
//            Assert.AreEqual("New name", user.Name);

//            // Remove
//            RemoveUser(user);
//            user = FindUser(user);
//            Assert.Null(user);
//        }

//        [Test]
//        public void FullNameAddValidationFail()
//        {
//            var user = InternalTestUtil.GetNewUser();
//            user.Name = "Maria";

//            var error = Assert.Throws<ContextFluentValidationException>(() =>
//            {
//                AddUser(user);
//            });

//            Assert.NotNull(error);
//            Assert.AreEqual(1, error.Inconsistencies.Count);
//            Assert.IsAssignableFrom<UiFieldRequiredFluentValidationException>(error.Inconsistencies.First());
//            var validationError = (FluentPropertyValidationException)error.Inconsistencies.First();
//            Assert.AreEqual("Full Name", validationError.Values.First());
//        }

//        [Test]
//        public void FullNameUpdateValidationFail()
//        {
//            var user = InternalTestUtil.GetNewUser();
//            user.Name = "Maria Santos";

//            AddUser(user);
//            user = FindUser(user);
//            Assert.NotNull(user);

//            // Update
//            user.Name = "Maria";

//            var error = Assert.Throws<ContextFluentValidationException>(() =>
//            {
//                UpdateUser(user);
//            });

//            Assert.NotNull(error);
//            Assert.AreEqual(1, error.Inconsistencies.Count);
//            Assert.IsAssignableFrom<UiFieldRequiredFluentValidationException>(error.Inconsistencies.First());
//            var validationError = (FluentPropertyValidationException)error.Inconsistencies.First();
//            Assert.AreEqual("Full Name", validationError.Values.First());

//            // Remove
//            RemoveUser(user);
//        }

//        [Test]
//        public void FullNameAddAndUpdateValidationSuccess()
//        {
//            var user = InternalTestUtil.GetNewUser();
//            user.Name = "Maria Santos";

//            // Add
//            AddUser(user);
//            user = FindUser(user);
//            Assert.NotNull(user);

//            // Update
//            user.Name = "New name";
//            UpdateUser(user);
//            user = FindUser(user);
//            Assert.NotNull(user);
//            Assert.AreEqual("New name", user.Name);

//            // Remove
//            RemoveUser(user);
//        }
//    }
//}

