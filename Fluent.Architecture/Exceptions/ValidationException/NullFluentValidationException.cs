// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Newtonsoft.Json;

namespace dn32.infra.Exceptions.ValidationException
{
    /// <inheritdoc />
    public class NullParameterDnValidationException : NullValueDnValidationException
    {
        public NullParameterDnValidationException(string parameter)
            : base($"The parameter {parameter} can not be null.", parameter)
        {
        }

        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "NullParameterDnValidationException";
    }

    /// <inheritdoc />
    public class AlterLossOfDadaValidationException : DnValidationException
    {
        public AlterLossOfDadaValidationException()
            : base($"This operation physically removes all data from the requested table. If you really want to do this, you should add to the request header the term \"APAGAR_TUDO=YES\"")
        {
        }
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "AlterLossOfDadaValidationException";
    }
}