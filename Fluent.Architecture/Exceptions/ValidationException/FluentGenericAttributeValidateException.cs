// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Newtonsoft.Json;
using System.Reflection;

namespace dn32.infra.Exceptions.ValidationException
{
    public class DnGenericAttributeValidateException : DnUiFieldValidationException
    {
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "DnGenericAttributeValidateException";

        public DnGenericAttributeValidateException(PropertyInfo property, bool globalizeValues, string message, string compositionProperty, string compositionFieldName) :
            base(property, globalizeValues, message, compositionProperty, compositionFieldName)
        {
        }
    }
}