// -----------------------------------------------------------------------
// <copyright company="Dn System">
//     Copyright © Dn System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using dn32.infra.Extensoes;
using Newtonsoft.Json;
using System.Reflection;

namespace dn32.infra.Exceptions.ValidationException
{
    public class DnPropertyValidationException : DnValidationException
    {
        [JsonProperty("property_name")]
        public string PropertyName { get; set; }

        [JsonIgnore]
        public PropertyInfo Property { get; set; }

        public DnPropertyValidationException(PropertyInfo property, bool globalizeValues, string message, string compositionProperty) : base(message, globalizeValues)
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
        public override string GlobalizationKey => "DnPropertyValidationException";
    }
}