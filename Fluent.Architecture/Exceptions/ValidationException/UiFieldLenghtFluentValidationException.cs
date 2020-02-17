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
    public class UiFieldLenghtFluenteValidationException : FluenteUiFieldValidationException
    {
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "TheFieldLenghtFluenteValidationException";

        public int Min { get; set; }

        public double Max { get; set; }

        public UiFieldLenghtFluenteValidationException(PropertyInfo property, string compositionProperty, string compositionFieldName) :
            base(property, true, $"The {(compositionFieldName == null ? property.GetUiPropertyName() : compositionFieldName + "." + property.GetUiPropertyName())} field has more or less characters than allowed.", compositionProperty, compositionFieldName)
        {
            var ret = property.GetPropertyRange();
            Min = ret?.min ?? 0;
            Max = ret?.max ?? 0;

            Field = property.GetUiPropertyName();
        }
    }
}