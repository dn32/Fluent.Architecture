// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Newtonsoft.Json;

namespace dn32.infra.Exceptions.ValidationException
{
    public class NullValueDnValidationException : DnValidationException
    {
        public NullValueDnValidationException(string message, string parameter = null) : base(message, false, parameter) { }

        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "NullValueDnValidationException";
    }
}