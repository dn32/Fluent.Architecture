// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Repository;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Validation
{
    public abstract class BaseValidation
    {
        /// <summary>
        /// A validação do serviço.
        /// </summary>
        protected virtual BaseService Service { get; set; }
    }
}