// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluent.Architecture.Exceptions.ValidationException;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Validation
{
    internal interface IFluentValidation
    {
        bool NullParameterOk { get; set; }
        bool KeyValuesOk { get; set; }
        //TransactionalService Service { get; set; }

        void AddInconsistency(FluentValidationException ex);
    }
}
