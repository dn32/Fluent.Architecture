// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using dn32.infra.nucleo.erros_de_validacao;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dn32.infra.Validation
{
    /// <inheritdoc />
    /// <summary>
    /// Retorno de validação padrão do sistema.
    /// </summary>
    [Serializable]
    public class ContextDnValidationException : Exception
    {
        public bool ValidationError => true;

        [JsonProperty("inconsistencies")]
        public List<DnErroDeValidacao> Inconsistencies { get; }

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
        public override string Message => string.Join("\n", this.Inconsistencies.Select(x => "* " + (x.MensagemDeGlobalizacao ?? x.Mensagem)).ToArray());

        /// <summary>
        /// Adiciona uma nova inconsistência ao contexto.
        /// </summary>
        /// <param Nome="exception">
        /// A inconsistência que deseja adicionar.
        /// </param>
        public void AddInconsistency(DnErroDeValidacao exception)
        {
            Inconsistencies.Add(exception);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextDnValidationException"/> class. 
        /// Inicializa o contexto de validação.
        /// </summary>
        public ContextDnValidationException() : base(string.Empty)
        {
            Inconsistencies = new List<DnErroDeValidacao>();
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

        protected ContextDnValidationException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }
    }
}
