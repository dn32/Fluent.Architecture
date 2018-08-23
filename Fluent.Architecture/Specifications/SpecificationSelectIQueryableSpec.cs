using System.Linq;
using Fluent.Architecture.Service;

namespace Fluent.Architecture.Specifications
{
    /// <inheritdoc />
    /// <summary>
    /// Especificação base para todas as especificações do sistema que tiverem a saida diferente da entrada.
    /// Geralmente o Spec possui em Select nesse caso.
    /// </summary>
    /// <typeparam name="TE">Tipo de entidade da especificação.</typeparam>
    /// <typeparam name="TO">Tipo de objeto de saida da especificação.</typeparam>
    public abstract class SpecificationSelectIQueryableSpec<TE, TO> : SpecificationBase
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
        public abstract IQueryable<TO> Spec(IQueryable<TE> query);

        /// <inheritdoc />
        protected SpecificationSelectIQueryableSpec(TransactionalService service) : base(service)
        {
        }
    }
}