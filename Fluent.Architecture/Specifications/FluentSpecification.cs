using System.Linq;
using Fluent.Architecture.Service;

namespace Fluent.Architecture.Specifications
{
    /// <inheritdoc />
    /// <summary>
    /// Especificação base para todas as especificações do sistema.
    /// </summary>
    /// <typeparam name="TE">Tipo de entidade da especificação.</typeparam>
    public abstract class FluentSpecification<TE> : BaseSpecification
    {
        /// <summary>
        /// A especificação.
        /// </summary>
        /// <param name="query">
        /// A referência à tabela/documento vinda do repositório.
        /// </param>
        /// <returns>
        /// A especificação criada.
        /// </returns>
        public abstract IQueryable<TE> Spec(IQueryable<TE> query);

        /// <inheritdoc />
        protected FluentSpecification(TransactionalService service) : base(service)
        {
        }
    }
}