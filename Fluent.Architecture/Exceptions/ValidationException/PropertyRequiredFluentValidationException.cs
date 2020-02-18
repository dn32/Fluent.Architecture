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
    public class UiFieldRequiredFluenteValidationException : FluenteUiFieldValidationException
    {
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "TheFieldMustHaveAValueForThisOperation";

        public UiFieldRequiredFluenteValidationException(PropertyInfo property, string compositionProperty, string compositionFieldName) :
            base(property, true, $"The field {(compositionFieldName == null ? property.GetUiPropertyName() : compositionFieldName + "." + property.GetUiPropertyName())} must have a value for this operation.", compositionProperty, compositionFieldName)
        {
        }
    }
}