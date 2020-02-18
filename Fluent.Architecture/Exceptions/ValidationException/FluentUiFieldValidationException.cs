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
    public class DnUiFieldValidationException : DnPropertyValidationException
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "DnUiFieldValidationException";

        public DnUiFieldValidationException(PropertyInfo property, bool globalizeValues, string message, string compositionProperty, string compositionFieldName) :
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