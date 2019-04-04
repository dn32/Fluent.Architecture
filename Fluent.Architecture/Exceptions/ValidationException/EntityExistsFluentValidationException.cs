// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
namespace Fluent.Architecture.Exceptions.ValidationException
{
    public class EntityExistsFluentValidationException : FluentValidationException
    {
        public EntityExistsFluentValidationException(string entityKeys) : base($"An entity with any of these keys already exists in the database: {entityKeys}", false, entityKeys)
        {
        }
    }
}