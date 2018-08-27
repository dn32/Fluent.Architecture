using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fluent.Architecture.Exception;
using Fluent.Architecture.Exception.ValidationException;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.SupportElements.Controllers;
using Fluent.Architecture.Test.SupportElements.Model;
using Fluent.Architecture.Test.SupportElements.Specifications;
using Fluent.Architecture.Test.TestTools;
using Fluent.Architecture.Validation;
using Xunit;

namespace Fluent.Architecture.Test.Test
{
    public class PropagationTest : FluentInternalTest
    {
        [Theory]
        [InlineData(nameof(StudentController.Add))]
        [InlineData(nameof(StudentController.Add2))]
        public void AddPropagationTest(string methodName)
        {
            var student = InternalTestUtil.GetNewStudent();
            student.Id = 0;

            TestUtil.Execute<Student>(StudentControllerInstance, methodName, student);
            var foundStudent = TestUtil.Execute<Student>(StudentControllerInstance, nameof(StudentController.Find), student);

            Assert.NotNull(foundStudent);
            Assert.Equal(student.GetAllDataOfObject(), foundStudent.GetAllDataOfObject());

            //Remove
            TestUtil.Execute<Student>(StudentControllerInstance, nameof(UserController.Remove), student);
        }

        [Fact]
        public void AddPropagationFullParameterIsNullFail()
        {
            var error = TestUtil.Execute<ContextFluentValidation>(StudentControllerInstance, nameof(StudentController.Spec2), null);
            Assert.NotNull(error);
            Assert.Single(error.Inconsistencies);
            Assert.IsAssignableFrom<NullParameterFluentValidationException>(error.Inconsistencies.First());
        }

        [Fact]
        public void AddPropagationOneParameter()
        {
            var student = InternalTestUtil.GetNewStudent();
            student.Id = 0;
            student.Name = "test";
            TestUtil.Execute<Student>(StudentControllerInstance, nameof(StudentController.Add), student);

            var spec = new StudentByNameSpec(StudentControllerInstance, student.Name);
            var students = TestUtil.Execute<List<Student>>(StudentControllerInstance, nameof(StudentController.Spec), spec);
            Assert.NotNull(students);
            Assert.NotEmpty(students);

            //Remove
            TestUtil.Execute<Student>(StudentControllerInstance, nameof(UserController.Remove), student);
        }

        [Fact]
        public void PropagationMethodNotFoundFailTest()
        {
            var ex = Assert.Throws<IncorrectDevelopmentException>(() => TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.NotFound), null));
            Assert.NotNull(ex);
            Assert.Equal("The NotFound method was not found in the service Fluent.Architecture.Test.SupportElements.User and repository Fluent.Architecture.Test.SupportElements.User", ex.Message);
        }

        [Fact]
        public void PropagationMethodNotFound2FailTest()
        {
            var ex = Assert.Throws<IncorrectDevelopmentException>(() => TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.NotFound2), null));
            Assert.NotNull(ex);
            Assert.Equal($"The InvokeMethod method was not found in the service Fluent.Architecture.Test.SupportElements.User and repository Fluent.Architecture.Test.SupportElements.User", ex.Message);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void ExistsSuccessPropagationTest(bool expectedExists, bool userSelectSpec)
        {
            var user = InternalTestUtil.GetNewUser();
            var passwordForFind = expectedExists ? user.Password : user.Password + "xpto";
            var spec = userSelectSpec ? new UserByPassword(UserControllerInstance, passwordForFind) as BaseSpecification<User> : new UserIdByPassword(UserControllerInstance, passwordForFind);

            //Add
            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Add), user);

            var exists = TestUtil.Execute<bool>(UserControllerInstance, nameof(UserController.Exists), spec);
            Assert.Equal(expectedExists, exists);

            //Remove
            TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.Remove), user);
        }



        [Fact]
        public void PropagateMethodTestBNullParameter()
        {
            JsonResult propagateMethodTestB(UserController controller)
            {
                var id = TestUtil.NextRandom();
                var ret = controller.PropagateMethodTestB(id, null);
                return ret;
            }


            var error = TestUtil.Execute<ContextFluentValidation>(UserControllerInstance, nameof(UserController.PropagateMethodTestB), new object[] { });


            Assert.NotNull(error);
            Assert.Single(error.Inconsistencies);
            Assert.IsAssignableFrom<NullParameterFluentValidationException>(error.Inconsistencies.First());
        }

        [Fact]
        public void PropagateMethodTestA()
        {
            var user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.PropagateMethodTestA), null);
            Assert.NotNull(user);
            Assert.Equal(1, user.Id);
        }

        [Fact]
        public void PropagateMethodTestB()
        {
            var id = TestUtil.NextRandom();
            var name = "My name B";
            var user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.PropagateMethodTestB), new object[] { id, name });
            Assert.NotNull(user);
            Assert.Equal(id, user.Id);
            Assert.Equal(name, user.Name);
        }

        [Fact]
        public void PropagateMethodTestC()
        {
            var id = TestUtil.NextRandom();
            var name = "My name C";
            var student = TestUtil.Execute<Student>(UserControllerInstance, nameof(UserController.PropagateMethodTestC), new object[] { id, name });
            Assert.NotNull(student);
            Assert.Equal(id, student.Id);
            Assert.Equal(name, student.Name);
        }

        [Fact]
        public void PropagateMethodTestD()
        {
            var id = TestUtil.NextRandom();
            var user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.PropagateMethodTestD), id);
            Assert.NotNull(user);
            Assert.Equal(id, user.Id);
        }

        [Fact]
        public void PropagateMethodTestE()
        {
            var id = TestUtil.NextRandom();
            var user = TestUtil.Execute<Student>(UserControllerInstance, nameof(UserController.PropagateMethodTestE), id);
            Assert.NotNull(user);
            Assert.Equal(id, user.Id);
        }

        [Fact]
        public void PropagateTestF()
        {
            var user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.PropagateTestF), null);
            Assert.NotNull(user);
            Assert.Equal(1, user.Id);
        }

        [Fact]
        public void PropagateTestG()
        {
            var id = TestUtil.NextRandom();
            var name = "My name G";
            var user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.PropagateTestG), new object[] { id, name });
            Assert.NotNull(user);
            Assert.Equal(id, user.Id);
            Assert.Equal(name, user.Name);
        }

        [Fact]
        public void PropagateTestH()
        {
            var id = TestUtil.NextRandom();
            var name = "My name H";
            var student = TestUtil.Execute<Student>(UserControllerInstance, nameof(UserController.PropagateTestH), new object[] { id, name });
            Assert.NotNull(student);
            Assert.Equal(id, student.Id);
            Assert.Equal(name, student.Name);
        }

        [Fact]
        public void PropagateTestI()
        {

            var id = TestUtil.NextRandom();
            var student = TestUtil.Execute<Student>(UserControllerInstance, nameof(UserController.PropagateTestI), id);
            Assert.NotNull(student);
            Assert.Equal(id, student.Id);
        }

        [Fact]
        public void PropagateTestJ()
        {
            var id = TestUtil.NextRandom();
            var user = TestUtil.Execute<User>(UserControllerInstance, nameof(UserController.PropagateTestJ), id);
            Assert.NotNull(user);
            Assert.Equal(id, user.Id);
        }
    }
}
