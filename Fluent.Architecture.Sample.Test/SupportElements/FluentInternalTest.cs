using System.Configuration;
using Fluent.Architecture.Sample.Test.SupportElements.Controllers;
using Fluent.Architecture.Test.Mock;

namespace Fluent.Architecture.Sample.Test.SupportElements
{
    public class FluentInternalTest
    {
        public UserController UserControllerInstance { get; set; }

        public StudentController StudentControllerInstance { get; set; }

        public CourseController CourseControllerInstance { get; set; }

        public FluentInternalTest()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            Architecture.Test.Setup.Initialize(connectionString);
            this.UserControllerInstance = MockUtil.GetMockController<UserController>();
            this.StudentControllerInstance = MockUtil.GetMockController<StudentController>();
            this.CourseControllerInstance = MockUtil.GetMockController<CourseController>();
        }
    }
}