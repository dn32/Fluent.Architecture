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

    public class UiFieldRequiredFluentValidationException : FluentUiFieldValidationException
    {
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "TheFieldMustHaveAValueForThisOperation";

        public UiFieldRequiredFluentValidationException(PropertyInfo property, string compositionProperty, string compositionFieldName) :
            base(property, true, $"The field {(compositionFieldName == null ? property.GetUiPropertyName() : compositionFieldName + "." + property.GetUiPropertyName())} must have a value for this operation.", compositionProperty, compositionFieldName)
        {
        }
    }

    public class UiFieldLenghtFluentValidationException : FluentUiFieldValidationException
    {
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "TheFieldLenghtFluentValidationException";

        public int Min { get; set; }

        public double Max { get; set; }

        public UiFieldLenghtFluentValidationException(PropertyInfo property, string compositionProperty, string compositionFieldName) :
            base(property, true, $"The {(compositionFieldName == null ? property.GetUiPropertyName() : compositionFieldName + "." + property.GetUiPropertyName())} field has more or less characters than allowed.", compositionProperty, compositionFieldName)
        {
            var ret = property.GetPropertyRange();
            Min = ret?.min ?? 0;
            Max = ret?.max ?? 0;

            Field = property.GetUiPropertyName();
        }
    }
}