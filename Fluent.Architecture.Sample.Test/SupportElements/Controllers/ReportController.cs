// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System.Web.Mvc;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Services;

namespace Fluent.Architecture.Sample.Test.SupportElements.Controllers
{
    public class ReportController : FluentServiceController<ReportService>
    {
        public JsonResult GenerateError()
        {
            var result = Service.GenerateError();
            return Json(result);
        }
    }
}

