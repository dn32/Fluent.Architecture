// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Newtonsoft.Json;

namespace Fluente.Arquitetura.Exceptions.ValidationException
{
    public class EntityHasNotSearchableAttributeProperties : FluenteValidationException
    {
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "EntityHasNotSearchableAttributeProperties";

        public EntityHasNotSearchableAttributeProperties(string entityName)
            : base($"Entity {entityName} has no properties decorated with SearchableAttribute")
        {
        }
    }
}