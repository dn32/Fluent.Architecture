// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Fluent.Architecture.Sample.Test.SupportElements.Validations;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Sample.Test.SupportElements.Services
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
