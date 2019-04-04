// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
namespace Fluent.Architecture.Exceptions.ValidationException
{
    /// <inheritdoc />
    public class NullFluentValidationException : NullValueFluentValidationException
    {
        public NullFluentValidationException(string parameter)
            : base($"The parameter {parameter} can not be null.", parameter)
        {
        }
    }
}