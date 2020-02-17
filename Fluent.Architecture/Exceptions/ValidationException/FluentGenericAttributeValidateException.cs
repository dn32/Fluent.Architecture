// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Newtonsoft.Json;
using System.Reflection;

namespace Fluente.Arquitetura.Exceptions.ValidationException
{
    public class FluenteGenericAttributeValidateException : FluenteUiFieldValidationException
    {
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "FluenteGenericAttributeValidateException";

        public FluenteGenericAttributeValidateException(PropertyInfo property, bool globalizeValues, string message, string compositionProperty, string compositionFieldName) :
            base(property, globalizeValues, message, compositionProperty, compositionFieldName)
        {
        }
    }
}