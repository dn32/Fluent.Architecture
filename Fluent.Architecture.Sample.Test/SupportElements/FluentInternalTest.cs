using Fluent.Architecture.Sample.Test.SupportElements.Controllers;
using Fluent.Architecture.Test.Mock;

namespace Fluent.Architecture.Sample.Test.SupportElements
{
    public class FluentInternalTest
    {
        public UserController UserControllerInstance { get; set; }

        public StudentController StudentControllerInstance { get; set; }

        public FluentInternalTest()
        {
            Architecture.Test.Setup.Initialize();
            this.UserControllerInstance = MockUtil.GetMockController<UserController>();
            this.StudentControllerInstance = MockUtil.GetMockController<StudentController>();
        }
    }
}