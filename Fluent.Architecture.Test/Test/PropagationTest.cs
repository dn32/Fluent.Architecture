using System.Linq;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Exception.ValidationException;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Services;
using Fluent.Architecture.Test.SupportElements.Controllers;
using Fluent.Architecture.Test.SupportElements.Mock;
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
            var error = TestUtil.Execute<ContextFluentValidation>(StudentControllerInstance, nameof(StudentController.Spec2));
            Assert.NotNull(error);
            Assert.Single(error.Inconsistencies);
            Assert.IsAssignableFrom<NullParameterFluentValidationException>(error.Inconsistencies.First());
        }

        [Fact]
        public void AddPropagationOneParameterIsNullFail()
        {
            var spec = new StudentByTitleSpec(StudentControllerInstance, "test");
            var error = TestUtil.Execute<ContextFluentValidation>(StudentControllerInstance, nameof(StudentController.Spec), spec);
            Assert.NotNull(error);
            Assert.Single(error.Inconsistencies);
            Assert.IsAssignableFrom<FluentParameterValidationException>(error.Inconsistencies.First());
        }
    }
}
