// -----------------------------------------------------------------------
// <copyright company="Dn System">
//     Copyright © Dn System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using dn32.infra.Services;

namespace dn32.infra.Validation
{
    public abstract class BaseValidation
    {
        /// <summary>
        /// A validação do serviço.
        /// </summary>
        protected virtual BaseService Service { get; set; }
    }
}