// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Controller.Test.Services;
using Fluent.Architecture.Controllers;

namespace Fluent.Architecture.Controller.Test.Controllers
{
    public class ReportController : FluentServiceController<ReportService>
    {
        public string GenerateError()
        {
          return Service.GenerateError();
        }
    }
}

