// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using dn32.infra.nucleo.erros_de_validacao;

namespace dn32.infra.Validation
{
    internal interface IDnValidation
    {
        bool NullParameterOk { get; set; }
        bool KeyValuesOk { get; set; }
        //TransactionalService Servico { get; set; }

        void AddInconsistency(DnErroDeValidacao ex);
    }
}
