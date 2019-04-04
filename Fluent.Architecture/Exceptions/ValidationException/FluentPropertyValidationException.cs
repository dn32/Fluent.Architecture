// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Newtonsoft.Json;

namespace Fluent.Architecture.Exceptions.ValidationException
{
    public class FluentPropertyValidationException : FluentValidationException
    {
        [JsonProperty("property_name")]
        public string PropertyName { get; set; }

        public FluentPropertyValidationException(string propertyName, bool globalizeValues, string message) : base(message, globalizeValues, propertyName)
        {
            this.PropertyName = propertyName;
        }

        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "ThePropertyMustHaveAValueForThisOperation";
    }
}