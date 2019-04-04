// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

namespace Fluent.Architecture.Exceptions.ValidationException
{
    public class NullValueFluentValidationException : FluentValidationException
    {
        public NullValueFluentValidationException(string message, string parameter = null) : base(message, false, parameter) { }
    }
}