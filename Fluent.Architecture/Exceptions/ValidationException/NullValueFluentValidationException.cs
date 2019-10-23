// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Newtonsoft.Json;

namespace Fluent.Architecture.Exceptions.ValidationException
{
    public class NullValueFluentValidationException : FluentValidationException
    {
        public NullValueFluentValidationException(string message, string parameter = null) : base(message, false, parameter) { }
    
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "NullValueFluentValidationException";
    }
}