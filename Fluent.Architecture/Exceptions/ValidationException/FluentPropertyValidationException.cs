// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluente.Arquitetura.Extensoes;
using Newtonsoft.Json;
using System.Reflection;

namespace Fluente.Arquitetura.Exceptions.ValidationException
{
    public class FluentePropertyValidationException : FluenteValidationException
    {
        [JsonProperty("property_name")]
        public string PropertyName { get; set; }

        [JsonIgnore]
        public PropertyInfo Property { get; set; }

        public FluentePropertyValidationException(PropertyInfo property, bool globalizeValues, string message, string compositionProperty) : base(message, globalizeValues)
        {
            Property = property;
            if (string.IsNullOrWhiteSpace(compositionProperty))
            {
                PropertyName = property.GetJsonPropertyName();
            }
            else
            {
                PropertyName = $"{compositionProperty}.{property.GetJsonPropertyName()}";
            }

            Values = new[] { PropertyName };
        }

        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "FluentePropertyValidationException";
    }
}