// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
namespace Fluent.Architecture.Exceptions.ValidationException
{
    public class UniqueKeyFluentValidationException : FluentPropertyValidationException
    {
        public UniqueKeyFluentValidationException(string propertyName, string value) : base(propertyName, false, $"A record already exists in the database with the value {value} for the {propertyName}.")
        {
        }
    }
}