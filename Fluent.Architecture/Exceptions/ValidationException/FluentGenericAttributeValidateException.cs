// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Newtonsoft.Json;
using System.Reflection;

namespace Fluent.Architecture.Exceptions.ValidationException
{
    public class FluentGenericAttributeValidateException : FluentUiFieldValidationException
    {
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "FluentGenericAttributeValidateException";

        public FluentGenericAttributeValidateException(PropertyInfo property, bool globalizeValues, string message, string compositionProperty, string compositionFieldName) :
            base(property, globalizeValues, message, compositionProperty, compositionFieldName)
        {
        }
    }
}