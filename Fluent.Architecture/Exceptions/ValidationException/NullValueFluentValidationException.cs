// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Newtonsoft.Json;

namespace Fluente.Arquitetura.Exceptions.ValidationException
{
    public class NullValueFluenteValidationException : FluenteValidationException
    {
        public NullValueFluenteValidationException(string message, string parameter = null) : base(message, false, parameter) { }
    
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "NullValueFluenteValidationException";
    }
}