using System.Linq;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Exception.ValidationException;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Service;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.StudentTests.Specifications;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests;
using Fluent.Architecture.Test.Mock;
using Fluent.Architecture.Validation;
using Xunit;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest.StudentTests
{
    public class StudentTest
    {
        #region SETUP

        public TransactionalService Service { get; set; }
        public BaseController Controller { get; set; }

        public StudentTest()
        {
            Setup.Initialize();
            Controller = MockUtil.GetMockController(typeof(StudentController));
            Service = ServiceFactory.Create<FluentService<User>>(MockUtil.GetHttpContext());
        }

        #endregion

        [Theory]
        [InlineData(nameof(StudentController.Add))]
        [InlineData(nameof(StudentController.Add2))]
        public void AddPropagationTest(string methodName)
        {
            var student = StudentTestUtil.GetNew();
            student.Id = 0;

            TestUtil.Execute<Student>(typeof(StudentController), methodName, student);
            var foundStudent = TestUtil.Execute<Student>(typeof(StudentController), nameof(StudentController.Find), student);

            Assert.NotNull(foundStudent);
            Assert.Equal(student.GetAllDataOfObject(), foundStudent.GetAllDataOfObject());
         
            //Remove
            TestUtil.Execute<Student>(typeof(StudentController), nameof(UserController.Remove), student);
        }

        [Fact]
        public void AddPropagationFullParameterIsNullFail()
        {
            var error = TestUtil.Execute<ContextFluentValidation>(typeof(StudentController), nameof(StudentController.Spec2));
            Assert.NotNull(error);
            Assert.Single(error.Inconsistencies);
            Assert.IsAssignableFrom<NullParameterFluentValidationException>(error.Inconsistencies.First());
        }

        [Fact]
        public void AddPropagationOneParameterIsNullFail()
        {
            var spec = new StudentByTitleSpec(Service, "test");
            var error = TestUtil.Execute<ContextFluentValidation>(typeof(StudentController), nameof(StudentController.Spec), spec);
            Assert.NotNull(error);
            Assert.Single(error.Inconsistencies);
            Assert.IsAssignableFrom<FluentParameterValidationException>(error.Inconsistencies.First());
        }
    }
}
