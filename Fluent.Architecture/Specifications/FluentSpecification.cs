// ReSharper disable CommentTypo

using System;
using System.Linq;
using System.Linq.Expressions;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Model;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Specifications
{
    /// <inheritdoc />
    /// <summary>
    /// Especificação base para todas as especificações do sistema.
    /// </summary>
    /// <typeparam name="TE">Tipo de entidade da especificação.</typeparam>
    public abstract class FluentSpecification<TE> : BaseSpecification<TE> where TE : BaseEntity
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
        public abstract IQueryable<TE> Where(IQueryable<TE> query);

        // Todo documentar
        public abstract Expression<Func<TE, object>> Order();

        /// <inheritdoc />
        protected FluentSpecification(TransactionalService service) : base(service) { }

        /// <inheritdoc />
        protected FluentSpecification(FluentController<TE> controller) : base(controller) { }

        internal IQueryable<TE> ToIQueryable(IQueryable<TE> query)
        {
            return this.Where(query).OrderBy(this.Order());
        }
    }
}