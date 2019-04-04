// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

namespace Fluent.Architecture.Exceptions.ValidationException
{
    public class FluentGlobalizeValidationException : FluentValidationException
    {
        public FluentGlobalizeValidationException(string globalizationKey, bool globalizeValues = false, params string[] parameters) : base("validation error", globalizationKey, globalizeValues, parameters)
        {
        }
    }
}