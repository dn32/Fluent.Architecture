// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluent.Architecture.Extensions;
using Newtonsoft.Json;
using System.Reflection;

namespace Fluent.Architecture.Exceptions.ValidationException
{
    public class FluentPropertyValidationException : FluentValidationException
    {
        [JsonProperty("property_name")]
        public string PropertyName { get; set; }

        [JsonIgnore]
        public PropertyInfo Property { get; set; }

        public FluentPropertyValidationException(PropertyInfo property, bool globalizeValues, string message, string compositionProperty) : base(message, globalizeValues)
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
        public override string GlobalizationKey => "ThePropertyMustHaveAValueForThisOperation";
    }
}