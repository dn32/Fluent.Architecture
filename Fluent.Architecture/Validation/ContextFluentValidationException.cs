// ReSharper disable CommentTypo
using System.Collections.Generic;
using System.Linq;
using Fluent.Architecture.Exceptions.ValidationException;

namespace Fluent.Architecture.Validation
{
    /// <inheritdoc />
    /// <summary>
    /// Retorno de validação padrão do sistema.
    /// </summary>
    public class ContextFluentValidationException : FluentValidationException
    {
        public ICollection<FluentValidationException> Inconsistencies { get; }

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
        public override string Message => string.Join("\n", this.Inconsistencies.Select(x => x.Message).ToArray());

        /// <summary>
        /// Adiciona uma nova inconsistência ao contexto.
        /// </summary>
        /// <param name="exception">
        /// A inconsistência que deseja adicionar.
        /// </param>
        public void AddInconsistency(FluentValidationException exception)
        {
            Inconsistencies.Add(exception);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextFluentValidationException"/> class. 
        /// Inicializa o contexto de validação.
        /// </summary>
        public ContextFluentValidationException() : base(string.Empty)
        {
            Inconsistencies = new List<FluentValidationException>();
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
    }
}
