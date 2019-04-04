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
    public class FluentParameterValidationException : FluentValidationException
    {
        public string Parameter { get; set; }

        public FluentParameterValidationException(string parameter, string message) : base(message, false, parameter)
        {
            this.Parameter = parameter;
        }
    }
}