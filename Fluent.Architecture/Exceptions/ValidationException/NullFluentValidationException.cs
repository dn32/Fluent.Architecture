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
    /// <inheritdoc />
    public class NullParameterFluentValidationException : NullValueFluentValidationException
    {
        public NullParameterFluentValidationException(string parameter)
            : base($"The parameter {parameter} can not be null.", parameter)
        {
        }

        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "NullParameterFluentValidationException";
    }

    /// <inheritdoc />
    public class AlterLossOfDadaValidationException : FluentValidationException
    {
        public AlterLossOfDadaValidationException()
            : base($"This operation physically removes all data from the requested table. If you really want to do this, you should add to the request header the term \"ERASE_ALL_DATA=YES\"")
        {
        }
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "AlterLossOfDadaValidationException";
    }
}