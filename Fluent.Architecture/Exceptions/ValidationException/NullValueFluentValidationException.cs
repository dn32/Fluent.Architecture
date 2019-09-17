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
        public NullValueFluentValidationException(string message, string parameter) : base(message, false, parameter) { }
        public NullValueFluentValidationException(string message) : base(message, false) { }
    }
}