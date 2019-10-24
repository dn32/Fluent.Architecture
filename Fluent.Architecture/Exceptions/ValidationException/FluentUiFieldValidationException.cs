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
    public class FluentUiFieldValidationException : FluentPropertyValidationException
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "FluentUiFieldValidationException";

        public FluentUiFieldValidationException(PropertyInfo property, bool globalizeValues, string message, string compositionProperty, string compositionFieldName) :
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