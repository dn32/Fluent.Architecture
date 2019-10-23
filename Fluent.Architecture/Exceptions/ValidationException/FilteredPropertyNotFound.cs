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
    public class FilteredPropertyNotFound : FluentValidationException
    {
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "FilteredPropertyNotFound";

        public FilteredPropertyNotFound(string entityName, string propertyName)
            : base($"Entity {entityName} does not have a property with name {propertyName}")
        {
        }
    }
}