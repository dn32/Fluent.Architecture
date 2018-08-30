// ReSharper disable CommentTypo

using System.Linq;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Model;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Specifications
{
    /// <inheritdoc />
    /// <summary>
    /// Especificação base para todas as especificações do sistema que tiverem a saida diferente da entrada.
    /// Geralmente o SpecSelect possui em Select nesse caso.
    /// </summary>
    /// <typeparam name="TE">Tipo de entidade da especificação.</typeparam>
    /// <typeparam name="TO">Tipo de objeto de saida da especificação.</typeparam>
    public abstract class FluentSelectSpecification<TE, TO> : BaseSpecification<TE> where TE : BaseEntity
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

        internal IQueryable<TO> ToIQueryable(IQueryable<TE> query)
        {
            return this.Spec(query);
        }

        /// <inheritdoc />
        protected FluentSelectSpecification(FluentController<TE> controller) : base(controller){}

        /// <inheritdoc />
        protected FluentSelectSpecification(TransactionalService service) : base(service) { }
    }
}