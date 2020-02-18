// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Newtonsoft.Json;

namespace dn32.infra.Exceptions.ValidationException
{
    public class FilteredPropertyNotFound : FluenteValidationException
    {
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "FilteredPropertyNotFound";

        public FilteredPropertyNotFound(string entityName, string propertyName)
            : base($"Entity {entityName} does not have a property with name {propertyName}")
        {
        }
    }
}