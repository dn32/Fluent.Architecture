// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using dn32.infra.Exceptions.ValidationException;

namespace dn32.infra.Validation
{
    internal interface IFluenteValidation
    {
        bool NullParameterOk { get; set; }
        bool KeyValuesOk { get; set; }
        //TransactionalService Service { get; set; }

        void AddInconsistency(FluenteValidationException ex);
    }
}
