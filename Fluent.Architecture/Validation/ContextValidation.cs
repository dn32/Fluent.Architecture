using System.Collections.Generic;
using System.Linq;
using Fluent.Architecture.Exception.ValidationException;
using Fluent.Architecture.Extensions;

namespace Fluent.Architecture.Validation
{
    /// <inheritdoc />
    /// <summary>
    /// Retorno de validação padrão do sistema.
    /// </summary>
    public class ContextValidation: ValidationException
    {
        public List<FluentValidationtException> Inconsistencies { get; set; }

        /// <summary>
        /// Se a validação retornou sucesso.
        /// </summary>
        public bool IsValid => Inconsistencies.Count == 0;

        /// <summary>
        /// Se a validação retornou falha.
        /// </summary>
        public bool IsInvalid => !IsValid;

        /// <inheritdoc />
        /// <summary>
        /// A mensagem de erro da falidação em caso de falha,
        /// </summary>
        public override string Message => string.Join("\n", Inconsistencies.Select(x => x.Message).ToArray());

        /// <summary>
        /// Adiciona uma nova inconsistência ao contexto.
        /// </summary>
        /// <param name="exception">
        /// A inconsistência que deseja adicionar.
        /// </param>
        public void AddInconsistency(FluentValidationtException exception)
        {
            Inconsistencies.Add(exception);
        }
        
        /// <summary>
        /// Inicializa o contexto de validação.
        /// </summary>
        public ContextValidation():base("")
        {
            Inconsistencies = new List<FluentValidationtException>();
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
