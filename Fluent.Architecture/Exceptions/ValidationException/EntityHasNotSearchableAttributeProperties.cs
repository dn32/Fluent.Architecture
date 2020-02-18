// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Newtonsoft.Json;

namespace dn32.infra.Exceptions.ValidationException
{
    public class EntityHasNotSearchableAttributeProperties : DnValidationException
    {
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "EntityHasNotSearchableAttributeProperties";

        public EntityHasNotSearchableAttributeProperties(string entityName)
            : base($"Entidade {entityName} has no properties decorated with SearchableAttribute")
        {
        }
    }
}