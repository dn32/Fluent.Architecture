using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Exceptions.ValidationException;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Sample.Test.SupportElements;
using Fluent.Architecture.Sample.Test.SupportElements.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Sample.Test.SupportElements.Specifications;
using Fluent.Architecture.Sample.Test.TestTools;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test;
using Fluent.Architecture.Validation;
using NUnit.Framework;

namespace Fluent.Architecture.Sample.Test.Test
{

    [TestFixture]
    [ComVisible(true)]
    public class PropagationTest : FluentInternalTest
    {
        [Theory]
        [TestCase(nameof(StudentController.Add))]
        [TestCase(nameof(StudentController.Add2))]
        public void AddPropagationTest(string methodName)
        {
            var student = InternalTestUtil.GetNewStudent();
            student.Id = 0;

            TestUtil.Execute<Student>(this.StudentControllerInstance, methodName, student);
            var foundStudent = TestUtil.Execute<Student>(this.StudentControllerInstance, nameof(StudentController.Find), student);

            Assert.NotNull(foundStudent);
            Assert.AreEqual(student.GetAllDataOfObject(), foundStudent.GetAllDataOfObject());

            // Remove
            TestUtil.Execute<Student>(this.StudentControllerInstance, nameof(UserController.Remove), student);
        }

        [Test]
        public void AddPropagationFullParameterIsNullFail()
        {
            var error = TestUtil.Execute<ContextFluentValidationException>(this.StudentControllerInstance, nameof(StudentController.Spec2), null);
            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.IsAssignableFrom<NullParameterFluentValidationException>(error.Inconsistencies.First());
        }

        [Test]
        public void AddPropagationOneParameter()
        {
            var student = InternalTestUtil.GetNewStudent();
            student.Id = 0;
            student.Name = "test";
            TestUtil.Execute<Student>(this.StudentControllerInstance, nameof(StudentController.Add), student);

            var spec = new StudentByNameSpec(this.StudentControllerInstance, student.Name);
            var students = TestUtil.Execute<List<Student>>(this.StudentControllerInstance, nameof(StudentController.Spec), spec);
            Assert.NotNull(students);
            Assert.IsNotEmpty(students);

            // Remove
            TestUtil.Execute<Student>(this.StudentControllerInstance, nameof(UserController.Remove), student);
        }

        [Test]
        public void PropagationMethodNotFoundFailTest()
        {
            var ex = Assert.Throws<IncorrectDevelopmentException>(() => TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.NotFound), null));
            Assert.NotNull(ex);
            Assert.AreEqual("The NotFound method was not found in the service Fluent.Architecture.Test.SupportElements.User and repository Fluent.Architecture.Test.SupportElements.User", ex.Message);
        }

        [Test]
        public void AmbiguousMethodOnRepositoryMatchException()
        {
            bool Spec(BaseController controller)
            {
                ((UserController)controller).Test();
                return true;
            }

            var ex = Assert.Throws<IncorrectDevelopmentException>(() => TestUtil.Execute(this.UserControllerInstance, null, null, Spec));
            Assert.NotNull(ex);
            Assert.AreEqual("There are two or more methods of propagation in Fluent.Architecture.Sample.Test.SupportElements.UserRepository with the same name Test. This causes an ambiguity, please change the name of one of them.", ex.Message);
        }

        [Test]
        public void AmbiguousMethodOnValidationMatchException()
        {
            bool Spec(BaseController controller)
            {
                ((UserController)controller).Test2();
                return true;
            }

            var ex = Assert.Throws<IncorrectDevelopmentException>(() => TestUtil.Execute(this.UserControllerInstance, null, null, Spec));
            Assert.NotNull(ex);
            Assert.AreEqual("There are two or more methods of propagation in  with the same name Test2. This causes an ambiguity, please change the name of one of them.", ex.Message);
        }

        // [Test]
        // public void PropagationMethodNotFound2FailTest()
        // {
        // var ex = Assert.Throws<IncorrectDevelopmentException>(() => TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.NotFound2), null));
        // Assert.NotNull(ex);
        // Assert.AreEqual($"The InvokeMethod method was not found in the service Fluent.Architecture.Test.SupportElements.User and repository Fluent.Architecture.Test.SupportElements.User", ex.Message);
        // }
        [Theory]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void ExistsSuccessPropagationTest(bool expectedExists, bool userSelectSpec)
        {
            var user = InternalTestUtil.GetNewUser();
            var passwordForFind = expectedExists ? user.Password : user.Password + "xpto";
            var spec = userSelectSpec ? new UserByPassword(this.UserControllerInstance, passwordForFind) as BaseSpecification<User> : new UserIdByPassword(this.UserControllerInstance, passwordForFind);

            // Add
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Add), user);

            var exists = TestUtil.Execute<bool>(this.UserControllerInstance, nameof(UserController.Exists), spec);
            Assert.AreEqual(expectedExists, exists);

            // Remove
            TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.Remove), user);
        }

        [Test]
        public void PropagateMethodTestBNullParameter()
        {
            ContextFluentValidationException PropagateMethodTestB(BaseController controller)
            {
                var id = TestUtil.NextRandom();
                var ret = Assert.Throws<ContextFluentValidationException>(() => ((UserController)controller).PropagateMethodTestB(id, null));
                return ret;
            }

            var error = TestUtil.Execute(this.UserControllerInstance, null, null, PropagateMethodTestB);

            Assert.NotNull(error);
            Assert.AreEqual(1, error.Inconsistencies.Count);
            Assert.NotNull(error.Message);
            Assert.IsAssignableFrom<FluentParameterValidationException>(error.Inconsistencies.First());
            var ex = error.Inconsistencies.First() as FluentParameterValidationException;

            Assert.NotNull(ex);
            Assert.AreEqual("parameters", ex.Parameter);
        }

        [Test]
        public void PropagateMethodTestA()
        {
            var user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.PropagateMethodTestA), null);
            Assert.NotNull(user);
            Assert.AreEqual(1, user.Id);
        }

        [Test]
        public void PropagateParameterCountFail()
        {
            IncorrectDevelopmentException ParameterCountFail(BaseController controller)
            {
                return Assert.Throws<IncorrectDevelopmentException>(() => ((UserController)controller).ParameterCountFail());
            }

            var error = TestUtil.Execute(this.UserControllerInstance, null, null, ParameterCountFail);

            Assert.NotNull(error);
            Assert.AreEqual("The amount of parameters passed is greater than the amount expected by the method.", error.Message);
        }

        [Test]
        public void PropagateMethodTestB()
        {
            var id = TestUtil.NextRandom();
            var name = "My name B";
            var user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.PropagateMethodTestB), new object[] { id, name });
            Assert.NotNull(user);
            Assert.AreEqual(id, user.Id);
            Assert.AreEqual(name, user.Name);
        }

        [Test]
        public void PropagateMethodTestC()
        {
            var id = TestUtil.NextRandom();
            var name = "My name C";
            var student = TestUtil.Execute<Student>(this.UserControllerInstance, nameof(UserController.PropagateMethodTestC), new object[] { id, name });
            Assert.NotNull(student);
            Assert.AreEqual(id, student.Id);
            Assert.AreEqual(name, student.Name);
        }

        [Test]
        public void PropagateMethodTestD()
        {
            var id = TestUtil.NextRandom();
            var user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.PropagateMethodTestD), id);
            Assert.NotNull(user);
            Assert.AreEqual(id, user.Id);
        }

        [Test]
        public void PropagateMethodTestE()
        {
            var id = TestUtil.NextRandom();
            var user = TestUtil.Execute<Student>(this.UserControllerInstance, nameof(UserController.PropagateMethodTestE), id);
            Assert.NotNull(user);
            Assert.AreEqual(id, user.Id);
        }

        [Test]
        public void PropagateTestF()
        {
            var user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.PropagateTestF), null);
            Assert.NotNull(user);
            Assert.AreEqual(1, user.Id);
        }

        [Test]
        public void PropagateTestG()
        {
            var id = TestUtil.NextRandom();
            var name = "My name G";
            var user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.PropagateTestG), new object[] { id, name });
            Assert.NotNull(user);
            Assert.AreEqual(id, user.Id);
            Assert.AreEqual(name, user.Name);
        }

        [Test]
        public void PropagateTestH()
        {
            var id = TestUtil.NextRandom();
            var name = "My name H";
            var student = TestUtil.Execute<Student>(this.UserControllerInstance, nameof(UserController.PropagateTestH), new object[] { id, name });
            Assert.NotNull(student);
            Assert.AreEqual(id, student.Id);
            Assert.AreEqual(name, student.Name);
        }

        [Test]
        public void PropagateTestI()
        {

            var id = TestUtil.NextRandom();
            var student = TestUtil.Execute<Student>(this.UserControllerInstance, nameof(UserController.PropagateTestI), id);
            Assert.NotNull(student);
            Assert.AreEqual(id, student.Id);
        }

        [Test]
        public void PropagateTestJ()
        {
            var id = TestUtil.NextRandom();
            var user = TestUtil.Execute<User>(this.UserControllerInstance, nameof(UserController.PropagateTestJ), id);
            Assert.NotNull(user);
            Assert.AreEqual(id, user.Id);
        }
    }
}
