// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using dn32.infra.Extensoes;
using Newtonsoft.Json;
using System.Reflection;

namespace dn32.infra.Exceptions.ValidationException
{
    public class FluenteUiFieldValidationException : FluentePropertyValidationException
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "FluenteUiFieldValidationException";

        public FluenteUiFieldValidationException(PropertyInfo property, bool globalizeValues, string message, string compositionProperty, string compositionFieldName) :
            base(property, globalizeValues, message, compositionProperty)
        {
            if (string.IsNullOrWhiteSpace(compositionFieldName))
            {
                Field = property.GetUiPropertyName();
            }
            else
            {
                Field = $"{compositionFieldName}.{property.GetUiPropertyName()}";
            }
        }
    }
}