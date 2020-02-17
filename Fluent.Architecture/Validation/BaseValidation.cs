// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluente.Arquitetura.Services;

namespace Fluente.Arquitetura.Validation
{
    public abstract class BaseValidation
    {
        /// <summary>
        /// A validação do serviço.
        /// </summary>
        protected virtual BaseService Service { get; set; }
    }
}