using Fluent.Architecture.Test.SupportElements.Controllers;
using Fluent.Architecture.Test.SupportElements.Mock;

namespace Fluent.Architecture.Test.SupportElements
{
    public class FluentInternalTest
    {
        public UserController UserControllerInstance { get; set; }
        public StudentController StudentControllerInstance { get; set; }

        public FluentInternalTest()
        {
            Setup.Initialize();
            this.UserControllerInstance = MockUtil.GetMockController<UserController>();
            this.StudentControllerInstance = MockUtil.GetMockController<StudentController>();
        }
    }
}