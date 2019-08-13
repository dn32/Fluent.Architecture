// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
namespace Fluent.Architecture.Exceptions.ValidationException
{
    public class EntityHasNotSearchableAttributeProperties : FluentValidationException
    {
        public EntityHasNotSearchableAttributeProperties(string entityName)
            : base($"Entity {entityName} has no properties decorated with SearchableAttribute")
        {
        }
    }
}