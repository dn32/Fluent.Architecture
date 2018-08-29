// ReSharper disable CommentTypo

using System.Collections.Generic;
using System.Linq;

namespace Fluent.Architecture.Validation
{
    using System.Diagnostics.CodeAnalysis;

    using Fluent.Architecture.Exceptions.ValidationException;

    /// <inheritdoc />
    /// <summary>
    /// Retorno de validação padrão do sistema.
    /// </summary>
    public class ContextFluentValidation : FluentValidationException
    {
        public List<FluentValidationException> Inconsistencies { get; }

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
            this.Inconsistencies.Add(exception);
        }

        /// <summary>
        /// Inicializa o contexto de validação.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1642:ConstructorSummaryDocumentationMustBeginWithStandardText", Justification = "Reviewed. Suppression is OK here.")]
        public ContextFluentValidation() : base("")
        {
            this.Inconsistencies = new List<FluentValidationException>();
        }

        /// <summary>
        /// Executa a validação, disparando a exceção em caso de falha.
        /// </summary>
        public void Validate()
        {
            if (this.IsInvalid)
            {
                throw this;
            }
        }
    }
}
