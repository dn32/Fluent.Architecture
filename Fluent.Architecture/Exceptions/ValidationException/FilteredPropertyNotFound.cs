// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
namespace Fluent.Architecture.Exceptions.ValidationException
{
    public class FilteredPropertyNotFound : FluentValidationException
    {
        public FilteredPropertyNotFound(string entityName, string propertyName)
            : base($"Entity {entityName} does not have a property with name {propertyName}")
        {
        }
    }
}