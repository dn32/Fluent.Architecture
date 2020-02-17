// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluente.Arquitetura.Exceptions.ValidationException;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluente.Arquitetura.Validation
{
    /// <inheritdoc />
    /// <summary>
    /// Retorno de validação padrão do sistema.
    /// </summary>
    [Serializable]
    public class ContextFluenteValidationException : Exception
    {
        public bool ValidationError => true;

        [JsonProperty("inconsistencies")]
        public List<FluenteValidationException> Inconsistencies { get; }

        /// <summary>
        /// Se a validação retornou sucesso.
        /// </summary>
        public bool IsValid => this.Inconsistencies.Count == 0;

        /// <summary>
        /// Se a validação retornou falha.
        /// </summary>
        public bool IsInvalid => !this.IsValid;

        /// <inheritdoc />
        /// <summary>
        /// A mensagem de erro da falidação em caso de falha,
        /// </summary>
        public override string Message => string.Join("\n", this.Inconsistencies.Select(x => "* " + (x.GlobalizedMessage ?? x.Message)).ToArray());

        /// <summary>
        /// Adiciona uma nova inconsistência ao contexto.
        /// </summary>
        /// <param name="exception">
        /// A inconsistência que deseja adicionar.
        /// </param>
        public void AddInconsistency(FluenteValidationException exception)
        {
            Inconsistencies.Add(exception);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextFluenteValidationException"/> class. 
        /// Inicializa o contexto de validação.
        /// </summary>
        public ContextFluenteValidationException() : base(string.Empty)
        {
            Inconsistencies = new List<FluenteValidationException>();
        }

        /// <summary>
        /// Executa a validação, disparando a exceção em caso de falha.
        /// </summary>
        public void Validate()
        {
            if (IsInvalid)
            {
                throw this;
            }
        }

        protected ContextFluenteValidationException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }
    }
}
