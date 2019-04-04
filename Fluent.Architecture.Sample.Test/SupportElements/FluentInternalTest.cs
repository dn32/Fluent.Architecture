// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System.Configuration;
using Fluent.Architecture.Sample.Test.SupportElements.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Services;
using Fluent.Architecture.Services;
using Fluent.Architecture.Test.Mock;

namespace Fluent.Architecture.Sample.Test.SupportElements
{
    public class FluentInternalTest
    {
        public UserController UserControllerInstance { get; set; }

        public StudentController StudentControllerInstance { get; set; }

        public CourseController CourseControllerInstance { get; set; }

        public ReportController ReportControllerInstance { get; set; }

        public FluentInternalTest()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            Fluent.Architecture.Test.Setup.Initialize(connectionString);
            Fluent.Architecture.Setup.SetGlobalizationServiceType<FluentGlobalizationService>(null);

            this.UserControllerInstance = MockUtil.GetMockController<UserController>();
            this.StudentControllerInstance = MockUtil.GetMockController<StudentController>();
            this.CourseControllerInstance = MockUtil.GetMockController<CourseController>();
            this.ReportControllerInstance = MockUtil.GetMockController<ReportController>();
        }
    }
}