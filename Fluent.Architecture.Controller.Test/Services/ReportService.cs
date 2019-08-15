// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Fluent.Architecture.Controller.Test.Validations;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Controller.Test.Services
{
    public class ReportService : TransactionalService
    {
        protected override Type ValidationType => typeof(ReportValidation);

        public string Generate()
        {
            return "";
        }

        public string GenerateError()
        {
            Validation.ValueMustBeInformed(null);
            return "";
        }
    }
}
