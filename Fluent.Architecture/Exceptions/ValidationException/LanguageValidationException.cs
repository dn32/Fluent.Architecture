// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

namespace Fluent.Architecture.Exceptions.ValidationException
{
    public class LanguageValidationException : FluentValidationException
    {
        public LanguageValidationException(string message) : base(message)
        {
        }
    }
}