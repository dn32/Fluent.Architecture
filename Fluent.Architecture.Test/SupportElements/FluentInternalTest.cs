using System.Web.Mvc;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Test.SupportElements.Controllers;
using Fluent.Architecture.Test.SupportElements.Mock;

namespace Fluent.Architecture.Test.Test
{
    public class FluentInternalTest
    {
        public UserController UserControllerInstance { get; set; }
        public StudentController StudentControllerInstance { get; set; }

        public FluentInternalTest()
        {
            Setup.Initialize();
            UserControllerInstance = MockUtil.GetMockController<UserController>();
            StudentControllerInstance = MockUtil.GetMockController<StudentController>();
        }
    }
}