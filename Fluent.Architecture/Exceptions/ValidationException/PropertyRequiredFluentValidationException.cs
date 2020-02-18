// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using dn32.infra.Extensoes;
using Newtonsoft.Json;
using System.Reflection;

namespace dn32.infra.Exceptions.ValidationException
{
    public class UiFieldRequiredDnValidationException : DnUiFieldValidationException
    {
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "TheFieldMustHaveAValueForThisOperation";

        public UiFieldRequiredDnValidationException(PropertyInfo property, string compositionProperty, string compositionFieldName) :
            base(property, true, $"The field {(compositionFieldName == null ? property.GetUiPropertyName() : compositionFieldName + "." + property.GetUiPropertyName())} must have a valor for this operation.", compositionProperty, compositionFieldName)
        {
        }
    }
}