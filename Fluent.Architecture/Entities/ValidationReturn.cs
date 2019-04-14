// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluent.Architecture.Exceptions.ValidationException;
using Fluent.Architecture.Validation;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fluent.Architecture.Entities
{
    public class ValidationReturn
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("validation_error")]
        public bool ValidationError { get; set; }
    }

    public class ValidationExceptionReturn : ValidationReturn
    {
        public ValidationExceptionReturn(ContextFluentValidationException validationException)
        {
            ValidationException = validationException;
        }

        [JsonProperty("inconsistencies")]
        public List<FluentValidationException> Inconsistencies => ValidationException?.Inconsistencies;

        [JsonProperty("validation_error")]
        public new bool ValidationError => ValidationException?.ValidationError ?? true;

        /// <summary>
        /// Se a validação retornou sucesso.
        /// </summary>
        [JsonProperty("is_valid")]
        public bool IsValid => ValidationException?.IsValid ?? false;

        /// <summary>
        /// Se a validação retornou falha.
        /// </summary>
        [JsonProperty("is_invalid")]
        public bool IsInvalid => ValidationException?.IsInvalid ?? true;

        /// <inheritdoc />
        /// <summary>
        /// A mensagem de erro da falidação em caso de falha,
        /// </summary>
        [JsonProperty("message")]
        public new string Message => ValidationException?.Message ?? string.Empty;

        [JsonIgnore]
        public ContextFluentValidationException ValidationException { get; set; }
    }
}