// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using dn32.infra.Nucleo.Inconsistences;
using System.Collections.Generic;

namespace dn32.infra.Nucleo.Models
{
    public class ValidationReturn
    {
        public string Message { get; set; }

        public bool ValidationError { get; set; }

        public List<FluenteInconsistence> Inconsistencies { get; set; }
    }

    //public class ValidationExceptionReturn : ValidationReturn
    //{
    //    public ValidationExceptionReturn(ContextFluenteValidationException validationException)
    //    {
    //        ValidationException = validationException;
    //    }

    //    [JsonProperty("inconsistencies")]
    //    public List<FluenteValidationException> Inconsistencies => ValidationException?.Inconsistencies;

    //    [JsonProperty("validation_error")]
    //    public new bool ValidationError => ValidationException?.ValidationError ?? true;

    //    /// <summary>
    //    /// Se a validação retornou sucesso.
    //    /// </summary>
    //    [JsonProperty("is_valid")]
    //    public bool IsValid => ValidationException?.IsValid ?? false;

    //    /// <summary>
    //    /// Se a validação retornou falha.
    //    /// </summary>
    //    [JsonProperty("is_invalid")]
    //    public bool IsInvalid => ValidationException?.IsInvalid ?? true;

    //    /// <inheritdoc />
    //    /// <summary>
    //    /// A mensagem de erro da falidação em caso de falha,
    //    /// </summary>
    //    [JsonProperty("message")]
    //    public new string Message => ValidationException?.Message ?? string.Empty;

    //    [JsonIgnore]
    //    public ContextFluenteValidationException ValidationException { get; set; }
    //}
}