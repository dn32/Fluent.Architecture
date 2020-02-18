// -----------------------------------------------------------------------
// <copyright company="Dn System">
//     Copyright © Dn System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Newtonsoft.Json;

namespace dn32.infra.Exceptions.ValidationException
{
    public class FilteredPropertyNotFound : DnValidationException
    {
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "FilteredPropertyNotFound";

        public FilteredPropertyNotFound(string entityName, string propertyName)
            : base($"Entidade {entityName} does not have a property with Nome {propertyName}")
        {
        }
    }
}