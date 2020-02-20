// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
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

        public List<DnInconsistence> Inconsistencies { get; set; }
    }

    //public class ValidationExceptionReturn : ValidationReturn
    //{
    //    public ValidationExceptionReturn(ContextDnValidationException validationException)
    //    {
    //        ValidationException = validationException;
    //    }

    //    [JsonProperty("inconsistencies")]
    //    public Listar<DnErroDeValidacao> Inconsistencies => ValidationException?.Inconsistencies;

    //    [JsonProperty("validation_error")]
    //    public new bool ErroDeValidacao => ValidationException?.ErroDeValidacao ?? true;

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
    //    [JsonProperty("mensagem")]
    //    public new string Mensagem => ValidationException?.Mensagem ?? string.Empty;

    //    [JsonIgnore]
    //    public ContextDnValidationException ValidationException { get; set; }
    //}
}