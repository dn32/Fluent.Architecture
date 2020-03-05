// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using dn32.infra.nucleo.servicos;
using dn32.infra.servicos;

namespace dn32.infra.Validation
{
    public abstract class BaseValidation
    {
        /// <summary>
        /// A validação do serviço.
        /// </summary>
        protected virtual DnServicoBase Service { get; set; }
    }
}