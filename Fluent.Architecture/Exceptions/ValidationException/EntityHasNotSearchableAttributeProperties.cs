// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Newtonsoft.Json;

namespace Fluent.Architecture.Exceptions.ValidationException
{
    public class EntityHasNotSearchableAttributeProperties : FluentValidationException
    {
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "EntityHasNotSearchableAttributeProperties";

        public EntityHasNotSearchableAttributeProperties(string entityName)
            : base($"Entity {entityName} has no properties decorated with SearchableAttribute")
        {
        }
    }
}